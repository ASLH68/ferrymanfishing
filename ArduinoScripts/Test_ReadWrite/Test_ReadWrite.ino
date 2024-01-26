//write lights
const int PIN_BUTTON1 = 8; //yellow
const int PIN_BUTTON2 = 2; //green

//read lights
const int PIN_READLED = 13; //red
int ledState = 0;

void setup() {
  Serial.begin(9600);

  pinMode(PIN_BUTTON1, INPUT);
  pinMode(PIN_BUTTON2, INPUT);

  pinMode(PIN_READLED, OUTPUT);
}

void loop() {
  WriteTest();
  //yellow button is pushed
  OutputFromButton(PIN_BUTTON1, 1);
    //green button is pushed
  OutputFromButton(PIN_BUTTON2, 2);  

  //Writing from unity test
}

void OutputFromButton(int pinButton, int writeValue)
{
  if(digitalRead(pinButton) == LOW)
  {
    Serial.write(writeValue);
    Serial.flush();
    delay(20);
  }
}

void WriteTest()
{
  ledState = ReceiveSerial();

  if(ledState == 1)
  {
    digitalWrite(PIN_READLED, HIGH);
  }
  else
  {
    digitalWrite(PIN_READLED, LOW);
  }

    delay(100);
}

int ReceiveSerial()
{
  if(Serial.available())
  {
    int serialData = Serial.read();
    Serial.println(serialData);
    switch(serialData)
    {
      case '1':
        return 1;

        break;
      case '0':
        return 0;

        break;
      default:
        return -1;
    }
  }
}
