const int BUTTON_L = 2; //Left button
const int BUTTON_R = 3; //Right button
const int LED_GREEN = 4; //Green LED
const int LED_RED = 5; //Red LED

void setup() {
  Serial.begin(19200);

  pinMode(BUTTON_L, INPUT);
  pinMode(BUTTON_R, INPUT);

  pinMode(LED_GREEN, OUTPUT);
  pinMode(LED_RED, OUTPUT);
}

void loop() {
  int pressedL = digitalRead(BUTTON_L);
  int pressedR = digitalRead(BUTTON_R);

  Serial.print(pressedL);
  Serial.print(",");
  Serial.print(pressedR);

  ActivateLight(LED_GREEN, pressedL);
  ActivateLight(LED_RED, pressedR);

  delay(50);
}

void ActivateLight(int pin, int digitalState)
{
  if(digitalState == 1)
  {
    digitalWrite(pin, HIGH);
  }
  else
  {
    digitalWrite(pin, LOW);
  }
}
