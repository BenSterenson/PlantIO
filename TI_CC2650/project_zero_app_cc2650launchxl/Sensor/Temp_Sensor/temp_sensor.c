/*
 * temp_sensor.c
 *
 *  Created on: Feb 9, 2017
 *      Author: Ben
 */


#include <driverlib/aon_batmon.h>
#include "../../Sensor/ex_include_tirtos.h"

void processTaskAlert_temp(void)
{
	int32_t temp = AONBatMonTemperatureGetDegC();

    //Log_info0("#########~TEMPERATURE SENSOR~#########");
    //Log_info1("Temp sensor value: %d", temp);
    //Log_info0("######################################");

    extern uint16_t globalValue;
    globalValue = (globalValue & 0xFF00) | (temp & 0x00FF);

} // processTaskAlert_temp
