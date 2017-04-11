import json
import requests
import matplotlib
import matplotlib.pyplot as plt
from matplotlib.dates import date2num
from datetime import datetime
import matplotlib.dates as mdates

class PlantIO():
    REST_API = 'https://sheetsu.com/apis/v1.0/5384ce7c1dc4'
    SOIL_MOISTURE = 'soilmoisture'
    LIGHT = 'light'
    TEMPERATURE = 'temperature'

    def __init__(self, rest_api_url = REST_API):
        self.Rest_Api = rest_api_url
        self.soil_moisture_data =[]
        self.light_data = []
        self.temp_data = []

    def read_json(self):
        response = requests.get(url = self.Rest_Api)
        json_data = json.loads(response.text)
        print json_data


        for row in json_data:

            if row['type'] == PlantIO.SOIL_MOISTURE:
                self.soil_moisture_data.append((row))

            elif row['type'] == PlantIO.LIGHT:
                self.light_data.append((row))

            elif row['type'] == PlantIO.TEMPERATURE:
                self.temp_data.append((row))

        self.soil_moisture_data.sort(key=lambda x:x['time_stamp'])
        self.light_data.sort(key=lambda x:x['time_stamp'])
        self.temp_data.sort(key=lambda x:x['time_stamp'])

    def strDateToDateTime(self, strDate):
        return datetime.strptime(strDate, '%d/%m/%Y %H:%M:%S')

    def generate_graph(self):
        dates = [self.strDateToDateTime(sm_dict['time_stamp']) for sm_dict in self.soil_moisture_data]

        plt.xticks(dates, dates, rotation=45)
        plt.plot(dates, [int(sm_dict['value']) for sm_dict in self.soil_moisture_data])
        plt.plot(dates, [int(light_dict['value']) for light_dict in self.soil_moisture_data])
        plt.plot(dates, [int(temp_dict['value']) for temp_dict in self.soil_moisture_data])

        plt.show()

if __name__ == '__main__':
    t = PlantIO()
    t.read_json()
    t.generate_graph()
