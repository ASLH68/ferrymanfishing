const int MOTOR_PIN = 3; 
const int BUTTON_PIN = 2; 

void setup() {
  Serial.begin(19200);

  pinMode(MOTOR_PIN, OUTPUT);
  pinMode(BUTTON_PIN, INPUT);
}

void loop() {
  int pressed = digitalRead(BUTTON_PIN);
  Serial.println(pressed);
  if(pressed == 0)
  {
    digitalWrite(MOTOR_PIN, LOW);
  }
  else if(pressed == 1)
  {
    digitalWrite(MOTOR_PIN, HIGH);
  }
}
