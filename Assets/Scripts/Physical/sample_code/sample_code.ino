
int i = 0;
const int maxTime = 200;

int buttonState = 0;  // 0 or 1
int buttonState2 = 0;
int buttonState3 = 0;

//const int buttonPin = 2;
//const int buttonPin2 = 3;
const int buttonPin3 = 4;
const int ledPin = 13;
const int pingPin = 7;
const int motorPin = 2;
const int servoPin = 2;
const int rumblePin = 3;
#define sensor A1
#include <Servo.h>
#include <SerialCommand.h>
#include <SoftwareSerial.h>

Servo servo;
SerialCommand sCmd;
String inputString = "";
bool stringComplete = false;

void setup() {
  // put your setup code here, to run once:
  pinMode(LED_BUILTIN, OUTPUT);
  // pinMode(buttonPin, INPUT);
  // pinMode(buttonPin2, INPUT);
  pinMode(buttonPin3, INPUT);
  pinMode(rumblePin, OUTPUT);
  // digitalWrite(buttonPin, HIGH);
  // digitalWrite(buttonPin2, HIGH);
  digitalWrite(buttonPin3, HIGH);
  servo.attach(servoPin);
  Serial.begin(9600);  //open serial port
  // sCmd.addCommand("GET_DISTANCE", distanceHandle);
  // sCmd.addCommand("SET_SERVO_180", servo180Handle);
  // sCmd.addCommand("SET_SERVO_90", servo90Handle);
  // sCmd.addCommand("SET_HAPTIC_HIGH", highHapticHandle);
  // sCmd.addCommand("SET_HAPTIC_LOW", lowHapticHandle);
  // sCmd.addCommand("SET_HAPTIC_OFF", offHapticHandle);
  // sCmd.addCommand("GET_BUTTON_STATE", buttonHandle);
}

void distanceHandle()
{
  Serial.write("DISTANCE:" + 2);
}

void buttonHandle()
{
  Serial.write("BUTTON!");
}

void servo180Handle()
{
  servo.write(180);
}

void servo90Handle()
{
  servo.write(90);
}

void highHapticHandle()
{

}

void lowHapticHandle()
{

}

void offHapticHandle()
{

}

void loop() {


 // buttonState = digitalRead(buttonPin);  //reading data in
    float volts = analogRead(sensor);  // value from sensor * (5/1024)
    String distance = String(volts);               // worked out from datasheet graph   

   if (Serial.available()) {
    // get the new byte:
    char inChar = (char)Serial.read();
    // add it to the inputString:
    inputString += inChar;
    // if the incoming character is a newline, set a flag so the main loop can
    // do something about it:
    if (inChar == '\n') {
      stringComplete = true;
    }
  }  
  
  int start = 0;
  int stop = 0;
  // print the string when a newline arrives:
  if (stringComplete) {
      start = inputString.substring(0, 2).toInt();
      stop = inputString.substring(3).toInt();
     Serial.write(inputString.c_str());
      Serial.write(distance.c_str());
      //Serial.write(buttonState);
      //Serial.flush();
    
    // clear the string:
    inputString = "";
    stringComplete = false;
  }

  servo.write(start);
  delay(100);
  servo.write(stop);
  delay(100);
// digitalWrite(rumblePin, HIGH);
  // delay(start);
  // digitalWrite(rumblePin, LOW);
  // delay(stop);
  // put your main code here, to run repeatedly:
  // digitalWrite(LED_BUILTIN, HIGH); //flash it to a high voltage
  // delay(200);
  // digitalWrite(LED_BUILTIN, LOW);
  // delay(200);
  // buttonState2 = digitalRead(buttonPin2);
  buttonState3 = digitalRead(buttonPin3);

  // if (buttonState2 == LOW) {
  //   float volts = analogRead(sensor) * 0.0068828125;  // value from sensor * (5/1024)
  //   int distance = 13 * pow(volts, -1);               // worked out from datasheet graph                                     // slow down serial port

  //   if (distance <= 30) {
  //     Serial.println(distance);  // print the distance
  //   }
  // }


  // if(buttonState == LOW)
  // {
  //   Serial.print("BRUH");
  // }
  digitalWrite(ledPin, 1 - buttonState);


  // if (buttonState3 == LOW) {

  //   servo.write(0);
  //   delay(1000);
  //   // Make servo go to 90 degrees
  //   servo.write(90);
  //   delay(1000);
  //   // Make servo go to 180 degrees
  //   servo.write(180);
  //   delay(1000);
  // }
  //  digitalWrite(motorPin, HIGH);
  //   delay(1000);
  // digitalWrite(motorPin, LOW);
  // delay(1000);
  //digitalWrite(ledPin, buttonState);
}
