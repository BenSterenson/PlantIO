# PlantIO - Documentation

# Description

* PlantIO is a smart IoT plant system that specializes in monitoring plants by sampling Soil Moisture, Temperature and Light levels.
* PlantIO was developed as part of Advanced Computer Systems Course.
* In order to simulate a complete system we use a combination of real sensor readings from a Soil Moisture sensor, board’s temperature and randomly generated light levels.

# Solving The Problem 

* Urban planting is getting more and more popular, many people grow spices and ornamental plants in their homes. Although the popularity, many don’t know how, or find it hard to remember to water the plant and give it the right amount of water in the right time.
* PlantIO is trying to solve this problem by monitoring each plant’s Soil Moisture, Temperature and Light exposure, giving the user a full picture of the plant, making the growing process easy and accurate.
* PlantIO is meant for any house that wants to grow some plants and get a better picture on the process of planting. Most of the smart solutions today are very complicated, expensive and energy-consuming.
* By using PlantIO the user does not waste water and can give the optimal amount that is needed to grow the plant in the best way. Furthermore, it is cheap and very energy-efficient way to monitor plants without the need to buy fancy robotic and computer systems.

# Equipment:

1. SparkFun Soil Moisture Sensor - https://www.sparkfun.com/products/13322
2. SimpleLink™ CC2650 Wireless MCU LaunchPad - http://www.ti.com/tool/launchxl-cc2650
3. Hook-Up Wire - Assortment https://www.sparkfun.com/products/11375
4. Break Away Headers - Straight https://www.sparkfun.com/products/116

# Hookup Guide:

1. Solder some wire to the sensor as following:
Use colors to distinguish between Voltage, Ground and Signal wires.
2. Cut 3 pins from the Break Away Headers and Solder the wire to them.
3. Plug the 3 pins to (5v, GND and DIO25 ports) Your final board should look as following:

# Software Setup:

1. Code Composer Studio - http://www.ti.com/tool/ccstudio
2. TI BLE Stack v2.2.1 - http://www.ti.com/tool/BLE-STACK-ARCHIVE
3. Visual Studio 2017 - Xamarin https://www.visualstudio.com
4. Python 2.7.X - https://www.python.org/
5. Python Matplotlib library - https://matplotlib.org/
6. Python Requests library - http://docs.python-requests.org/en/master/
7. Using google drive - Create a google sheet with the following column names:

8. Copy sheet Url to https://sheetsu.com
9. Copy API URL https://sheetsu.com/apis/v1.0/YOUR_API_ID and Enter API URL to PlantIO app.
10. Download project source files from https://github.com/BenSterenson/PlantIO

# High Level System Illustration:
From left to right:
* Sensors are inserted into the plant’s soil to sample the data
* The board transfers the measured data over a BLE protocol to a mobile phone
* The phone sends the data to a cloud-based database over a WiFi connection
* Data is provided from the cloud to the client over a WiFi connection to extract the information needed in order to display statistics and graphs

# Modules - Overview

- Texas Instruments CC2650 (Hardware):
Responsible for collecting data from Soil Moisture Sensor and board temperature (board temperature simulates the temperature the plant is exposed to).
Normalizes data into predetermined human-readable scale.
Sample and publish data via BLE - Supports periodic read/subscription to notification.
Subscription to notification - Once board identify 10 difference from last sample it will publish it to mobile client.

- Mobile Application (Gateway):
Used as a gateway to transfer measurements from CC2650 board to cloud.
Simple user interface supports periodic read and auto notification. 
Sends real time data to cloud.

- Google Drive (Cloud):
Responsible for storing measurements received from PlantIO application.
Interaction with google sheet by REST API.
Google sheet fields - id, date, time, type, scale, value, ble_sample_type

- Python Graph Plotting App (Client):
Responsible for extracting data and representing a graph with measurements.
Send mail with attached graph.

# Interfaces - Overview

Hardware - Gateway:
* Data is sent from Hardware to the Gateway using BLE Notification on every state change.
* Data is requested from Hardware by the Gateway using a BLE Read Request.

Gateway - Cloud:
* Data is uploaded through the Gateway to the Cloud over WiFi using REST API.

Cloud - Client:
* Data is downloaded from the Cloud by the Client over WiFi using REST API.

