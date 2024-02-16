const int BUTTON_L = 2; //Left button
const int BUTTON_R = 3; //Right button
const int LED_GREEN = 4; //Green LED
const int LED_RED = 5; //Red LED

unsigned long time = 0;
const unsigned long delayTime = 10;

int greenLightState;
int redLightState;
int builtinState;

void setup() {
  Serial.begin(19200);

  pinMode(BUTTON_L, INPUT);
  pinMode(BUTTON_R, INPUT);

  pinMode(LED_GREEN, OUTPUT);
  pinMode(LED_RED, OUTPUT);
  pinMode(LED_BUILTIN, OUTPUT);
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
    char data = (char)Serial.read();

    if(data == '1')
    {
      greenLightState = 1;
      redLightState = 0;
    }
    else if(data == '2')
    {
      greenLightState = 0;
      redLightState = 1;
    }
    else if(data == '0')
    {
      greenLightState = 0;
      redLightState = 0;
    }
    else if(data == '3')
    {
      builtinState = 1;
    }
    else if(data == '4')
    {
      builtinState = 0;
    }
    
    ActivateLight(LED_GREEN, greenLightState);
    ActivateLight(LED_RED, redLightState);
    ActivateLight(LED_BUILTIN, builtinState);
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
