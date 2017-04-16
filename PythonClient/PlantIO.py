import json
import requests
import matplotlib.pyplot as plt
from datetime import datetime
from datetime import timedelta
import matplotlib.dates as mdates

class PlantIO():
    REST_API = 'https://sheetsu.com/apis/v1.0/5384ce7c1dc4'
    SOIL_MOISTURE = 'soilmoisture'
    LIGHT = 'light'
    TEMPERATURE = 'temperature'

    def __init__(self, rest_api_url=REST_API):
        self.Rest_Api = rest_api_url
        self.soil_moisture_data = []
        self.light_data = []
        self.temp_data = []

    def read_json(self, date):
        url_request = self.Rest_Api + "/date/" + date.strftime("%d-%m-%Y")
        response = requests.get(url=url_request)
        json_data = json.loads(response.text)
        print json_data

        for row in json_data:

            if row['type'] == PlantIO.SOIL_MOISTURE:
                self.soil_moisture_data.append((row))

            elif row['type'] == PlantIO.LIGHT:
                self.light_data.append((row))

            elif row['type'] == PlantIO.TEMPERATURE:
                self.temp_data.append((row))

        #self.soil_moisture_data.sort(key=lambda x: x['time_stamp'])
        #self.light_data.sort(key=lambda x: x['time_stamp'])
        #self.temp_data.sort(key=lambda x: x['time_stamp'])

    def strDateToDateTime(self, strDate):
        return datetime.strptime(strDate, '%d-%m-%Y')

    def strTimeToDateTime(self, strDate):
        return datetime.strptime(strDate, '%H:%M').time()

    def generate_graph(self):
        today = datetime.today().replace(hour=0, minute=0, second=0, microsecond=0)
        self.generate_graph_day(today)

    def draw_graph(self, times, temp, moist, light):

        single_day_view = (times[-1] - times[0]) <= timedelta(days=1)

        if single_day_view:
            plt.gca().xaxis.set_major_formatter(mdates.DateFormatter('%H:%M'))
        else:
            plt.gca().xaxis.set_major_formatter(mdates.DateFormatter('%d-%m'))

        plt.gcf().autofmt_xdate(rotation=15)
        plt.xlim(times[0], times[-1])

        base_ax = plt.axes()
        plt.yticks([])
        plt.xlabel("Time", size=16)
        plt.title("Temperature, Light, and Moisture", size=16)

        base_box = base_ax.get_position()
        base_x = base_box.x0
        base_y = base_box.y0
        base_w = base_box.x1 - base_x
        base_h = base_box.y1 - base_y

        moist_ax = plt.axes([base_x, base_y + base_h * 1 / 3., base_w, base_h / 3.], frameon=False)
        moist_ax.yaxis.tick_right()  # the y values are on the right
        moist_ax.yaxis.set_label_position("right")
        plt.plot(times, moist, 'bo--')
        plt.ylabel('Moisture', size=14)
        plt.xticks([])  # hide x values
        plt.grid(True)
        plt.

        temp_ax = plt.axes([base_x, base_y + base_h * 2 / 3., base_w, base_h / 3.], frameon=False)
        temp_ax.yaxis.tick_left()  # the y values are on the left
        plt.plot(times, temp, 'ro--')
        plt.ylabel('Temperature (C)', size=14)
        plt.xticks([])  # hide x values
        plt.grid(True)

        light_ax = plt.axes([base_x, base_y + base_h * 0 / 3., base_w, base_h / 3.], frameon=False)
        light_ax.yaxis.tick_left()  # the y values are on the left
        plt.plot(times, light, 'go--')
        plt.ylabel('Light', size=14)
        plt.xticks([])  # hide x values
        plt.grid(True)

        # Legend
        plt.axes(base_ax)
        t_proxy = plt.Line2D([0], [0], c='#FF0000', lw=3)
        l_proxy = plt.Line2D([0], [0], c='#00FF00', lw=3)
        m_proxy = plt.Line2D([0], [0], c='#0000FF', lw=3)
        plt.legend((t_proxy, l_proxy, m_proxy), ("Temperature", "Light", "Moisture"))

        # plt.savefig("output.svg")
        plt.show()

    def generate_graph_day(self, day):

        times = []
        for measure in self.soil_moisture_data:
            date = self.strDateToDateTime(measure['date'])
            time = self.strTimeToDateTime(measure['time'])

            #if measure_time.date() == day.date():
            times.append(datetime.combine(date, time))

        temps = [int(measures['value']) for measures in self.temp_data]
        moists = [int(measures['value']) for measures in self.soil_moisture_data]
        lights = [int(measures['value']) for measures in self.light_data]

        self.draw_graph(times, temps, moists, lights)


if __name__ == '__main__':
    t = PlantIO()

    from_date = "16-04-2017"
    to_date = "17-04-2017"

    cur_date = t.strDateToDateTime(from_date)
    end_date = t.strDateToDateTime(to_date)
    while cur_date <= end_date:
        t.read_json(cur_date)
        cur_date += timedelta(days=1)
    t.generate_graph()

