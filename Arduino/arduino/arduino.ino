// IMU 參數
#include "MPU9250.h"
#define IMU IMU1
MPU9250 IMU(Wire, 0x68);
MPU9250 IMU2(Wire, 0x69);
int s; // 儲存IMU連接狀態

// 計算角度參數
#define BXYZ 2
#define DXYZ 0
long timeLast;
float angleDoor, angleBox;

// 矯正參數
int doorPin = 16; //門矯正點的pin，D0
int boxPins[] = {15, 13, 12, 14}; //0，90，180，270矯正點的pin，D8 D7 D6 D5
int denoise = 10;
int pinsCount, boxCC, doorCC; //儲存箱子矯正點的數量，

// Uduino 參數
#define UDUINO 2// 0: No uduino; 1: Uduino-Serial; 2: Uduino-WiFi
#if UDUINO == 1
#include "Uduino.h"
Uduino uduino("SerialMode");
#elif UDUINO == 2
#include "Uduino_Wifi.h"
Uduino_Wifi uduino("WifiMode");
#endif
bool uc;

// time elapse 參數
float mincount;
int captureIndex = 3;
#define mintoms 60000
#define threshold 2

// glitch 參數
float lastAngle;
int glitchIndex = 3;

void setup() {

  Serial.begin(115200);

  // set Pin mode
  pinMode(doorPin, INPUT);
  pinsCount = sizeof(boxPins) / sizeof(boxPins[0]);
  for (int i = 0; i < pinsCount; i++) {
    pinMode(boxPins[i], INPUT);
  }

#if UDUINO == 2
  // connect wifi
  uduino.connectWifi("InterBox", "12345678");
#endif

  // start communication with IMU
  int status = IMU.begin();
  s = status;
  if (s) {
    IMU.calibrateGyro();
  }
  uc = false;
}

void loop() {

#if UDUINO != 0
  uduino.update();
  if (uduino.isConnected()) {

    // Unity play進行0度矯正
    if (!uc) {
      IMU.calibrateGyro();
      angleDoor = 0;
      angleBox = 0;
      uc = true;
    }
#endif

    // 讀取Gyro到GyroXYZ[]
    float GyroXYZ[] = {0, 0, 0};
    if (s > 0) {
      IMU.readSensor();
      GyroXYZ[0] = IMU.getGyroX_rads();
      GyroXYZ[1] = IMU.getGyroY_rads();
      GyroXYZ[2] = IMU.getGyroZ_rads();
    }

    // 計算角度
    long timeNow = millis();
    float durSec = (timeNow - timeLast);
    angleDoor += GyroXYZ[DXYZ] * RAD_TO_DEG * durSec / 1000;
    angleBox += (cos(angleDoor / RAD_TO_DEG)*GyroXYZ[2] + sin(angleDoor / RAD_TO_DEG)*GyroXYZ[1]) * RAD_TO_DEG * durSec / 1000;
    angleBox += angleBox < 0 ? 360 : angleBox > 360 ? -360 : 0;
    timeLast = timeNow;

    // 箱子角度矯正：如果接觸到矯正點就設置角度變成0，90，180，270
    for (int i = 0; i < pinsCount; i++) {
      if (digitalRead(boxPins[i]) == 1) {
        boxCC++;
        angleBox = (360 / pinsCount) * i;
        break;
      }
      else {
        boxCC = 0;
      }
    }

    // 門的角度矯正：如果開門的話就把角度設成0
    if (digitalRead(doorPin) == 1) {
      doorCC++;
      angleDoor = 0;
    }
    else {
      doorCC = 0;
    }

    // 拍照計時器
    if((timeNow > mincount*mintoms) && (angleDoor > 30)){
      float diff = angleDoor - captureIndex*10 ;
      diff *= diff<0?-1:1;
      if(diff<threshold){
        captureIndex += captureIndex>8? -6: 1;
        mincount += timeNow/mintoms + 2;
      }
    }
    

    // 隨機glitch切換器
    int i = lastAngle/10;
    int j = angleDoor/10;
    if (j>=3 && j<= 9 && i != j){
      long randNumber = random(0, 10);
      if(randNumber < j){
        glitchIndex = j;
      }
    }

    lastAngle = angleDoor;

    // 傳送數據
    Serial.println(String(angleBox) + "\\" + String(angleDoor) + "\\" + String(captureIndex) + "\\" + String(glitchIndex));
#if UDUINO != 0
#if UDUINO == 2
    uduino.println(String(angleBox) + "\\" + String(angleDoor) + "\\" + String(captureIndex) + "\\" + String(glitchIndex));
#endif
  }
#endif
  delay(50);
}
