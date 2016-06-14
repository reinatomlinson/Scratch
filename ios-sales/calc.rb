require 'pp'
require 'httparty'

def extract_from dir
  Dir[dir].map do |filename|
    month_token = filename.split('_')[1]
    contents = File.open(filename, 'rb') { |f| f.read }
    contents.each_line
      .reject { |l| l =~ /Start Date/ }
      .reject { |l| l =~ /Total_/ }
      .map do |l|
      extract_line(l, month_token)
    end
  end.flatten
end

def token_map
  {
    0 =>  { label: :start_date, conversion: lambda { |v| v } },
    1 =>  { label: :end_date, conversion: lambda { |v| v } },
    2 =>  { label: :ups, conversion: lambda { |v| v } },
    3 =>  { label: :isrc, conversion: lambda { |v| v } },
    4 =>  { label: :vendor_id, conversion: lambda { |v| v } },
    5 =>  { label: :quantity, conversion: lambda { |v| v.to_i } },
    6 =>  { label: :partner_share, conversion: lambda { |v| v.to_f } },
    7 =>  { label: :extended_partner_share, conversion: lambda { |v| v.to_f } },
    8 =>  { label: :partner_share_currency, conversion: lambda { |v| v.to_f } },
    9 =>  { label: :sale_or_return, conversion: lambda { |v| v == "S" ? 1 : -1 } },
    10 => { label: :apple_id, conversion: lambda { |v| v } },
    11 => { label: :developer, conversion: lambda { |v| v } },
    12 => { label: :title, conversion: lambda { |v| v } },
    13 => { label: :publisher, conversion: lambda { |v| v } },
    14 => { label: :grid, conversion: lambda { |v| v } },
    15 => { label: :product_type_id, conversion: lambda { |v| v } },
    16 => { label: :isan, conversion: lambda { |v| v } },
    17 => { label: :country, conversion: lambda { |v| v } },
    18 => { label: :pre_order, conversion: lambda { |v| v } },
    19 => { label: :promo_code, conversion: lambda { |v| v } },
    20 => { label: :customer_price, conversion: lambda { |v| v.to_f } },
    21 => { label: :customer_currency, conversion: lambda { |v| v } }
  }
end

def extract_line l, month_token
  line_hash = Hash[
    l.strip
     .split("\t")
     .each_with_index
     .map { |v, i| [token_map[i][:label], token_map[i][:conversion].call(v)] }
  ]

  line_hash[:month] = month_token
  line_hash
end

sales = extract_from "./*_*_*.txt"

exchange_rates = HTTParty.get("http://api.fixer.io/latest?base=USD")["rates"]

converted = sales
  .sort_by { |s| [s[:vendor_id], s[:currency]] }
  .map do |s|
    exchange_rate = exchange_rates[s[:customer_currency]]
    exchange_rate = 1 if s[:customer_currency] == "USD"
    {
      :app => s[:vendor_id],
      :price => s[:customer_price],
      :currency => s[:customer_currency],
      :quantity => s[:quantity] * s[:sale_or_return],
      :exchange_rate => exchange_rate,
      :share => s[:partner_share],
      :month => s[:month]
    }
  end
  .reject { |s| s[:exchange_rate].nil? }
  .map do |s|
    net_usd = s[:share]
    net_usd = net_usd.to_f / s[:exchange_rate].to_f
    net_usd = net_usd * s[:quantity]
    s[:net_usd] = net_usd.round(2)
    s
  end

# converted.each { |s| puts s }

puts ""
puts "Here is the net sales. It should be pretty close to the amount Apple wired to you."
puts ""
puts converted.map { |s| s[:net_usd] }.inject(:+).round(2)

def total_usd_for app, month, sales
  filtered = sales
    .find_all { |s| s[:app] == app and s[:month] == month }
    .map { |s| s[:net_usd] } || [ ]

  (filtered.inject(:+) || 0).round(2)
end

def total_count_for app, month, sales
  filtered = sales
    .find_all { |s| s[:app] == app and s[:month] == month }
    .map { |s| s[:quantity] } ||  [ ]

  (filtered.inject(:+) || 0).round(2)
end

def average_for app, month, sales
  count = total_count_for(app, month, sales)
  return 0 if count == 0
  (total_usd_for(app, month, sales) / count.to_f).round(2)
end

puts ""
puts "Here is the breakdown by app."
puts ""

totals = { }

unique_months = converted.map { |s| s[:month] }.uniq

unique_months.each { |m| totals[m] = { } }

unique_months.each do |unique_month|
  totals[unique_month] = Hash[
    converted
    .map { |s| s[:app] }
    .uniq
    .map do |app|
      [
        app,
        {
          :total => total_usd_for(app, unique_month,  converted),
          :count => total_count_for(app, unique_month, converted),
          :avg => average_for(app, unique_month, converted)
        }
      ]
    end
  ]
end

totals.each do |k, v|
  v["michael's share"] = v["a-dark-room"][:total] * 0.5 + v["adr-bundle"][:total] * 0.25 + v["amirs-app"][:total] * 0.25
end

pp totals.map { |k, v| [k, v["michael's share"]] }.sort_by { |a| a[0][2..3] + a[0][0..1] }

=begin
Check for      Paid on      Amount     Calc Sales*    Calc Michael's    Actual Transfer    Transfered On
    04/15   06/04/2015    9,456.10        9489.54            3659.72           3,700.62       06/08/2015
    05/15   07/02/2015    7,933.61        7947.90            3153.28           3,165.48       07/13/2015
    06/15   07/30/2015   10,295.12       10509.04            3356.45           3,066.02       08/03/2015
    07/15   09/03/2015   18,634.44       20072.11            5434.97           5,441.44       09/18/2015
    08/15   10/01/2015    5,063.01        5253.33            1520.74           1,516.76       10/01/2015
    09/15   10/29/2015    9,004.61        9086.00            2016.56           1,998.00       11/02/2015
    10/15   12/03/2015    6,378.61        6537.15            2246.88           2,230.00       12/03/2015
    11/15   12/31/2015    4,658.01        4739.98            1683.25           1,648.29       01/07/2016
    12/15   01/28/2016    4,176.33        4283.84            1512.92           1,493.00       02/11/2016
    01/16   03/03/2016   10,429.41       10676.38            4097.37           4,059.00       03/09/2016
    02/16   03/31/2016    8,165.38        8246.77            3335.80           3,378.00       03/31/2016
    03/16   04/28/2016    7,428.56        7424.55            3015.57           3,034.00       05/02/2016
    04/16   06/02/2016   18,014.51       18286.55            7919.33
*rounding errors and currency converstion
=end
