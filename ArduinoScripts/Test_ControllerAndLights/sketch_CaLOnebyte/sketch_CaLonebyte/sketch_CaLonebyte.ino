const int BUTTON_L = 2; //Left button
const int BUTTON_R = 3; //Right button
const int LED_GREEN = 4; //Green LED
const int LED_RED = 5; //Red LED

unsigned long time = 0;
const unsigned long delayTime = 10;

int greenLightState;
int redLightState;

void setup() {
  Serial.begin(19200);

  pinMode(BUTTON_L, INPUT);
  pinMode(BUTTON_R, INPUT);

  pinMode(LED_GREEN, OUTPUT);
  pinMode(LED_RED, OUTPUT);
}

void loop() {
  RecieveSerialData();

  if(millis() > time)
  {
    SendSerialData();
    time = millis() + delayTime;
  }
}

void SendSerialData()
{
  int pressedL = digitalRead(BUTTON_L);
  int pressedR = digitalRead(BUTTON_R);
  int sendData = 0;

  if(pressedL)
  {
    sendData = 1;
  }
  else if(pressedR)
  {
    sendData = 2;
  }

  Serial.println(sendData);
}

void RecieveSerialData()
{
  if(Serial.available())
  {
    char message = (char)Serial.read();

    if(message == '1')
    {
      greenLightState = 1;
    }
    else if(message == '2')
    {
      greenLightState = 0;
    }

    if(message == '3')
    {
      redLightState = 1;
    }
    else if(message == '4')
    {
      redLightState = 0;
    }
    
    ActivateLight(LED_GREEN, greenLightState);
    ActivateLight(LED_RED, redLightState);
  }
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
