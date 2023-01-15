#include <BluetoothSerial.h>
#include <Wire.h>
#include "MAX30105.h"

BluetoothSerial BTSerial; 
MAX30105 particleSensor;

#define debug Serial

bool running;



// the setup routine runs on start and once when you press reset:
void setup() {

  // initialize serial communication at 9600 bits per second:
  BTSerial.begin("Pulse-Glove");
  Serial.begin(115200);
  pinMode(2, OUTPUT);
  if (particleSensor.begin() == false)
  {
    debug.println("MAX30105 was not found. Please check wiring/power. ");
    while (1);
  }

  particleSensor.setup(); //Configure sensor. Use 6.4mA for LED drive
  running = false;
}

// the loop routine runs over and over again forever:
void loop() {
  debug.println("Running loop");
  float temperature = particleSensor.readTemperature();
  float temperatureF = particleSensor.readTemperatureF(); //Because I am a bad global citizen
  debug.print(" C=");
  debug.print(temperature, 4);
  debug.print(" F=");
  debug.print(temperatureF, 4);
  debug.println();
  if (BTSerial.available() > 0 ){
    digitalWrite(2,HIGH);
    String data = BTSerial.readStringUntil('\n');
    if (data == "stop-hr"){
      digitalWrite(2, LOW);
      running = false;
			BTSerial.println("HR data transfer stopped");
		}
    else if (data == "start-hr"){
			digitalWrite(2, HIGH);
      running = true;
			BTSerial.println("HR data transfer started");
		}

    if (running) {
      BTSerial.print("H:");
      BTSerial.print(particleSensor.getIR()/6000);

      BTSerial.print(", C:");
      BTSerial.print(temperature, 4);

      BTSerial.print(", F:");
      BTSerial.print(temperatureF, 4);

      BTSerial.println();
    }
  } else {
    digitalWrite(2, LOW);
    delay(500);
    digitalWrite(2, HIGH);
    delay(500);
    debug.println("No device connected over Bluetooth");
  }
}

