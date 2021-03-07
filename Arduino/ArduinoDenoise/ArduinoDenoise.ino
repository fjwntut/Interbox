#define WIFIMODE

#include "MPU9250.h"
/*
#ifdef WIFIMODE
 #include<Uduino_Wifi.h>
  Uduino_Wifi uduino("WifiMode");

#else
  #include "Uduino.h"
  Uduino uduino("NormalMode");
#endif
*/
MPU9250 IMU1(Wire, 0x68);
MPU9250 IMU2(Wire, 0x69);
int status;
int s1, s2;
int closed = 12;

void setup() {

  Serial.begin(115200);

  pinMode(closed, INPUT);

  // check wifi name and password
  /*#ifdef WIFIMODE
    uduino.connectWifi("HaMsTeR🍆", "vava880810");
  #endif
*/
  // start communication with IMU
  status = IMU1.begin();
  s1 = status;
  status = IMU2.begin();
  s2 = status;

}

String IMUData(MPU9250 imu, int s) {

  String data = "";

  if (s > 0) { // connected
    imu.readSensor();

    data += String(imu.getGyroX_rads(), 6);
    data += "\\";
    data += String(imu.getGyroY_rads(), 6);
    data += "\\";
    data += String(imu.getGyroZ_rads(), 6);;
  }

  else {  // not connected
    data = "0\\0\\0";
  }

  return data;
}

void loop() {

  //uduino.update();
  
  // Combine data from 3 sensor
  String message = String(digitalRead(12)) + "\\" + IMUData(IMU1, s1) + "\\" + IMUData(IMU2, s2);
/*
  // Send to unity by com port 
  #ifdef WIFIMODE
    uduino.println(message);
  #else
    Serial.println(message);
  #endif
*/Serial.println(message);
  delay(100);
}
