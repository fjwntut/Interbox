// 矯正參數
int doorPin = 16; //門矯正點的pin，D0
int boxPins[] = {15, 14, 12, 13}; //0，90，180，270矯正點pin，D8 D5 D7 D6
int pinsCount, boxCC, doorCC; //儲存箱子矯正點的數量，箱子和門矯正的次數

void setup() {

  Serial.begin(115200);


  // set Pin mode
  pinMode(doorPin, INPUT);
  pinsCount = sizeof(boxPins) / sizeof(boxPins[0]);
  for (int i = 0; i < pinsCount; i++) {
    pinMode(boxPins[i], INPUT);
  }
}

void loop() {
  Serial.print(String(digitalRead(doorPin)));
  for (int i = 0; i < pinsCount; i++) {
    String reading = String(digitalRead(boxPins[i]));
    Serial.print(" " + reading);
  }
  Serial.println("test");
  delay(100);
}
