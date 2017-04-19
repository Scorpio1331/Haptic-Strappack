#include <Arduino.h> //Remove at release

//Change values to digital pins the motors are connected to
int motor1Pin = 2;
int motor2Pin = 3;
int motor3Pin = 4;
int motor4Pin = 6;

int bpm = 0;
float bps = 0;
char incomingByte;

void setup()
{
  //Set motor pins to output
  pinMode(motor1Pin, OUTPUT);
  pinMode(motor2Pin, OUTPUT);
  pinMode(motor3Pin, OUTPUT);
  pinMode(motor4Pin, OUTPUT);

  //Set motors to off
  digitalWrite(motor1Pin, LOW);
  digitalWrite(motor2Pin, LOW);
  digitalWrite(motor3Pin, LOW);
  digitalWrite(motor4Pin, LOW);


  //Beign receiving serial data at 9600 Baud rate
  Serial.begin(9600);
}

void loop()
{
  //Turn motors off
  digitalWrite(motor1Pin, LOW);
  digitalWrite(motor2Pin, LOW);
  digitalWrite(motor3Pin, LOW);
  digitalWrite(motor4Pin, LOW);
  
  //If bpm is greater than 0 loop through and turn motors on in turn
    if (bpm > 0) {    
      delay(bps);
      if(changeMotorState(motor1Pin)){ //If function returns true then serial data is available, so exit function
        return;
      }      
        
      if(changeMotorState(motor2Pin)){
        return;
      }       
        
      if(changeMotorState(motor3Pin)){
        return;
      }       
        
      if(changeMotorState(motor4Pin)){
        return;
      }
    }

}

boolean changeMotorState(int motorPin)
{    
    digitalWrite(motorPin, HIGH); //Turn motor on
    if (Serial.available()>0){ //If data is available on serial port stop loop 
      digitalWrite(motorPin, LOW);   
      return true;
    }
    delay(bps); //Wait for amount of time equal to the beats per second
    digitalWrite(motorPin, LOW); //Turn motor off
    return false;
}

//Serial event runs when data is available in serial connection and when no other function is running
void serialEvent()
{
  bpm = 0;         //Throw away previous bpm
  while(1) {            //Loop until 'n' is received
    incomingByte = Serial.read();
    if (incomingByte == '\n') break;   //If end of data, exit the while loop
    if (incomingByte == -1) continue;  //If no characters are in the buffer read() returns -1
    bpm *= 10;  //Shift bpm to the left by 1 decimal place
    //Convert read data from ASCII to integer, add, and shift left 1 decimal place
    bpm = ((incomingByte - 48) + bpm);
  }
  Serial.println(bpm);   //Print bpm, remove at release
  
 bps = (1.0/(bpm/60.0))*1000; //Divide 1 by bpm divided by 60 to get bps, then multiply by 1000 to get bps in milliseconds




}

