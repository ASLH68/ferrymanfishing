#include <Servo.h>
//button
const int buttonPin = 5;
int buttonState = 0;
bool buttonStateChanged = false;
//servo
#define ServoPin A0
Servo servo;
int servoRotation = 0;
bool servoChanged = false;
//rotary encoder
const int encoderCLK = 2;
const int encoderDT = 3;
int encoderInitState = 0;
int encoderState;
bool positiveIncreased = false;
bool negativeIncreased = false;
//haptic motor
const int rumblePin = 4;
bool hapticStateChanged = false;
int hapticState = 0;
unsigned long time = 0;
const unsigned long delayTime = 100;


const char HapticOn = 'a';
const char HapticOff = 'b';
const char ServoHigh = 'c';
const char ServoLow = 'd';
const char ButtonHigh = 'e';
const char ButtonLow = 'f';
const char EncoderIncreased = 'g';
const int ServoHighValue = 180;
const int ServoLowValue = 70;


void setup() {
  Serial.begin(19200);
  pinMode(buttonPin, INPUT);
  // buttonState = digitalRead(buttonPin);
  pinMode(encoderCLK, INPUT);
  pinMode(encoderDT, INPUT);
  pinMode(ServoPin, OUTPUT);
  pinMode(rumblePin, OUTPUT);
  pinMode(LED_BUILTIN, OUTPUT);
  servo.attach(ServoPin);
  encoderInitState = digitalRead(encoderCLK);
}

void loop() {

  RecieveSerialData();

  //gather data from hardware
  GatherHardwareData();
  //apply any data to hardware
  ApplyReceivedData();

  //send data
  if (millis() <= time) return;

  time = millis() + delayTime;
  SendSerialData();
}

int prevState = 0;
void GatherHardwareData() {
  //get whether button is pushed. Check for changes
  prevState = buttonState;
  buttonState = digitalRead(buttonPin);
  if (buttonState != prevState) {
    buttonStateChanged = true;
  }

  //read in the encoder values
  encoderState = digitalRead(encoderCLK);
  if (encoderState != encoderInitState && encoderState == HIGH) {
    if (digitalRead(encoderDT) != encoderState) {
      //positive incr
      positiveIncreased = true;
      negativeIncreased = false;
    } else {
      //negative decr
      negativeIncreased = true;
      positiveIncreased = false;
    }
  }
  encoderInitState = encoderState;
}


void ApplyReceivedData() {
  if (servoChanged) {
    servoChanged = false;
    servo.write(servoRotation);
  }

  if (hapticStateChanged) {
    hapticStateChanged = false;
    digitalWrite(rumblePin, hapticState);
  }
}


void SendSerialData() {
  //button is pushed
  bool dataSend = false;
  if (buttonStateChanged) {
    buttonStateChanged = false;
    if (buttonState == HIGH) {
      dataSend = true;
      Serial.print(ButtonHigh);
    } else {
      dataSend = true;
      Serial.print(ButtonLow);
    }
  }
  //rotatry encoder both ways
  if (positiveIncreased) {
    dataSend = true;
    positiveIncreased = false;
    Serial.print(EncoderIncreased);
  }

  if (dataSend) {
    Serial.println();
  }
}

void RecieveSerialData() {

  if (Serial.available()) {
    char message = (char)Serial.read();

    if (message == HapticOn) {
      int prev = hapticState;
      hapticState = HIGH;
      if (prev != hapticState) {
        hapticStateChanged = true;
      }
    } else if (message == HapticOff) {
      int prevHaptic = hapticState;
      hapticState = LOW;
      if (prevHaptic != hapticState) {
        hapticStateChanged = true;
      }
    } else if (message == ServoHigh) {
      int prevServo = servoRotation;
      servoRotation = ServoHighValue;
      if (prevServo != servoRotation) {
        servoChanged = true;
      }
    } else if (message == ServoLow) {
      int prevServo2 = servoRotation;
      servoRotation = ServoLowValue;
      if (prevServo2 != servoRotation) {
        servoChanged = true;
      }
    }

    //   switch (message) {
    //     case HapticOn:

    //       int prev = hapticState;
    //       hapticState = HIGH;
    //       if (prev != hapticState) {
    //         hapticStateChanged = true;
    //       }
    //       break;
    //     case HapticOff:

    //       int prevHaptic = hapticState;
    //       hapticState = LOW;
    //       if (prevHaptic != hapticState) {
    //         hapticStateChanged = true;
    //       }
    //       //do b shit
    //       break;
    //     case ServoHigh:
    //       int prevServo = servoRotation;
    //       servoRotation = ServoHighValue;
    //       if (prevServo != servoRotation) {
    //         servoChanged = true;
    //       }
    //       break;
    //     case ServoLow:
    //       int prevServo2 = servoRotation;
    //       servoRotation = ServoLowValue;
    //       if (prevServo2 != servoRotation) {
    //         servoChanged = true;
    //       }
    //       break;
    //     default:
    //       break;
    //   }
    // }
  }
}
