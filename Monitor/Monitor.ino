#include "ESP8266WiFi.h"
#include <ArduinoJson.h>
#include <DNSServer.h>
#include <ESP8266WebServer.h>
#include <WiFiManager.h>
#include <Ticker.h>
#include <TFT_22_ILI9225.h>
#include <../fonts/FreeSans9pt7b.h>
#include <../fonts/FreeSans8pt7b.h>

#define TFT_RST         4
#define TFT_RS          13
#define TFT_CS          5  // SS
#define TFT_SDI         2  // MOSI
#define TFT_CLK         15  // SCK
#define TFT_LED         0   // 0 if wired to +5V directly
#define TFT_BRIGHTNESS  200
#define cpuAMD          0xF324
#define gpuAMD          0xD846
#define cpuIntel        0x1B96
#define gpuNvidia       0x6540
#define ram             0xE89D
#define flash           0

String cpuName, cpuLoad, cpuTemp, ramLoad, gpuName, gpuLoad, gpuTemp;
String oldCPULoad, oldCPUTemp, oldRAMLoad, oldGPULoad, oldGPUTemp;
int timer, demFlash;
bool lanDauMoApp = true, ketNoiApp, doiWiFi;
uint16_t cpuColor, gpuColor;

WiFiServer server(80);
WiFiManager wifiManager;
WiFiClient client;
TFT_22_ILI9225 tft = TFT_22_ILI9225(TFT_RST, TFT_RS, TFT_CS, TFT_SDI, TFT_CLK, TFT_LED);
Ticker flipper;

void setup() {
  Serial.begin(115200);
  pinMode(flash,INPUT_PULLUP);
  pinMode(2,OUTPUT);
  digitalWrite(2,LOW);
  tft.setGFXFont(&FreeSans8pt7b);
  tft.begin();
  tft.clear();
  tft.setOrientation(2);
  tft.drawGFXText(10, 65, "Connecting to WiFi", COLOR_WHITE);
  flipper.attach(1, caiDat);
  wifiManager.autoConnect("HME","12345678");
  while (WiFi.status() != WL_CONNECTED) {
    delay(100);
    Serial.print(".");
  }
  flipper.detach();
  flipper.attach(1, ngat);
  tft.clear();
  tft.drawGFXText(35, 25, "App not open", COLOR_RED);
  tft.drawGFXText(33, 45, "TCP server IP", COLOR_WHITE);
  tft.drawGFXText(35, 65, WiFi.localIP().toString(), COLOR_WHITE);
  server.begin();
}

void loop() {
  if(ketNoiApp)
    getData();
  else {
    tft.drawGFXText(35, 25, "App not open", COLOR_RED);
    tft.drawGFXText(33, 45, "TCP server IP", COLOR_WHITE);
    tft.drawGFXText(35, 65, WiFi.localIP().toString(), COLOR_WHITE);
    if (!client.connected()) {
      client = server.available();
    }
    else {
      tft.clear();
      ketNoiApp   = true;
      lanDauMoApp = true;
      tft.setGFXFont(&FreeSans9pt7b);
    }
  }
  if(doiWiFi) {
    WiFi.disconnect();
    server.close();
    tft.clear();
    tft.setGFXFont(&FreeSans8pt7b);
    tft.drawGFXText(5, 40, "Connect to WiFi HME", COLOR_WHITE);
    tft.drawGFXText(33, 55, "with password", COLOR_WHITE);
    tft.drawGFXText(45, 70, "12345678", COLOR_WHITE);
    tft.drawGFXText(55,85, "to config", COLOR_WHITE);
    tft.drawGFXText(54,100, "new WiFi", COLOR_WHITE);
    wifiManager.autoConnect("HME","12345678");
    tft.clear();
    server.begin();
    tft.setGFXFont(&FreeSans9pt7b);
    doiWiFi = false;
  }
}

void caiDat() {
  timer++;
  if(timer == 20) {
    tft.drawGFXText(10, 65, "Connecting to WiFi", COLOR_BLACK);
    tft.drawGFXText(25, 25, "Connection Fail", COLOR_WHITE);
    tft.drawGFXText(5, 40, "Connect to WiFi HME", COLOR_WHITE);
    tft.drawGFXText(33, 55, "with password", COLOR_WHITE);
    tft.drawGFXText(45, 70, "12345678", COLOR_WHITE);
    tft.drawGFXText(55,85, "to config", COLOR_WHITE);
    tft.drawGFXText(54,100, "new WiFi", COLOR_WHITE);
    flipper.detach();
  }
}

void ngat() {
  if(!doiWiFi) {
    if(WiFi.status() != WL_CONNECTED)
      WiFi.reconnect();
    if(digitalRead(flash)) demFlash = 0;
    if(!digitalRead(flash) && demFlash<601) demFlash++;
    if(demFlash == 10) doiWiFi = true;
  }
}

void hienThiNhietDo(int x, int y, String data, uint16_t color) {
  tft.drawGFXText(x, y, data, color);
  if(data.length()>2){
    x+=11;
  }
  x+=22;
  tft.drawGFXText(x, y, "%", color);
  tft.drawLine(x+5, y, x+12, y-12, COLOR_BLACK);
  tft.drawPixel(x+8, y-6, COLOR_BLACK);
  tft.drawLine(x+10, y, x+10, y-5, COLOR_BLACK);
  tft.drawLine(x+11, y, x+11, y-5, COLOR_BLACK);
  tft.drawLine(x+12, y, x+12, y-5, COLOR_BLACK);
  tft.drawLine(x+13, y, x+13, y-5, COLOR_BLACK);
  tft.drawLine(x+14, y, x+14, y-5, COLOR_BLACK);
  tft.drawLine(x+15, y, x+15, y-5, COLOR_BLACK);
  tft.drawLine(x+16, y, x+16, y-5, COLOR_BLACK);
  tft.drawGFXText(x+10, y, "C", color);
}

void border() {
  cpuName.toUpperCase();
  gpuName.toUpperCase();
  if(cpuName.indexOf("AMD") == 0)
    cpuColor = cpuAMD;
  else if(cpuName.indexOf("INTEL") == 0)
    cpuColor = cpuIntel;
  if(gpuName.indexOf("AMD") == 0) 
    gpuColor = gpuAMD;
  else if(gpuName.indexOf("NVIDIA") == 0)
    gpuColor = gpuNvidia;
  tft.fillCircle(13,123,5,COLOR_BLUE);
  tft.fillCircle(28,114,5,COLOR_BLUE);
  tft.fillCircle(160,126,5,COLOR_BLUE);
  tft.fillCircle(145,117,5,COLOR_BLUE);
  tft.fillRectangle(10, 135, 70, 180, COLOR_RED);
  tft.fillTriangle(50, 135, 70, 135, 70, 180 , COLOR_BLACK);
  tft.fillRectangle(100, 135, 160, 180, COLOR_RED);
  tft.fillTriangle(100, 135, 120, 135, 100, 180 , COLOR_BLACK);
  
  tft.fillRectangle(33, 175, 80, 220, COLOR_RED);
  tft.fillTriangle(80, 175, 80, 190, 70, 175 , COLOR_BLACK);
  
  tft.fillRectangle(94, 175, 138, 220, COLOR_RED);
  tft.fillTriangle(94, 175, 94, 190, 104, 175 , COLOR_BLACK);
  
  
  tft.drawLine(0,0,35,0,cpuColor);
  tft.drawLine(35,0 ,41,6,cpuColor);
  tft.drawLine(41,6 ,86,6,cpuColor);
  tft.drawLine(86,71,86,6,cpuColor);
  tft.drawLine(86,71,40,104,cpuColor);
  tft.drawLine(40,104,18,66,cpuColor);
  tft.drawLine(18,66,10,66,cpuColor);
  tft.drawLine(10,66,0,50,cpuColor);
  tft.drawLine(0,50,0,0,cpuColor);

  tft.drawLine(88,6,133,6,gpuColor);
  tft.drawLine(133,6,139,0,gpuColor);
  tft.drawLine(139,0,173,0,gpuColor);
  tft.drawLine(173,0,173,50,gpuColor);
  tft.drawLine(173,50,163,67,gpuColor);
  tft.drawLine(163,67,155,67,gpuColor);
  tft.drawLine(155,67,133,104,gpuColor);
  tft.drawLine(88,71,88,6,gpuColor);
  tft.drawLine(88,71,133,104,gpuColor);
  
  tft.drawLine(133,106,109,144,ram);
  tft.drawLine(109,144,110,154,ram);
  tft.drawLine(110,154,85,194,ram);
  tft.drawLine(41,105,62,144,ram);
  tft.drawLine(62,144,61,153,ram);
  tft.drawLine(61,153,85,194,ram);
  tft.drawLine(87,73,41,105,ram);
  tft.drawLine(87,73,133,106,ram);
  
  
  tft.drawGFXText(3, 23, "CPU", cpuColor);
  tft.drawGFXText(3, 49, cpuLoad+"%", cpuColor);
  oldCPULoad = cpuLoad;
  hienThiNhietDo(30, 75, cpuTemp, cpuColor);
  oldCPUTemp = cpuTemp;
  
  tft.drawGFXText(67, 105, "RAM", ram);
  tft.drawGFXText(60, 126, ramLoad+"%", ram);
  oldRAMLoad = ramLoad;

  tft.drawGFXText(130, 23, "GPU", gpuColor);
  tft.drawGFXText(110, 49, gpuLoad+"%", gpuColor);
  oldGPULoad = gpuLoad;
  hienThiNhietDo(100, 75, gpuTemp, gpuColor);
  oldGPUTemp = gpuTemp;
}

void getData() {
  if (client.connected()) {
    if (client.available()>0) {
      String data = client.readStringUntil('\n');
      DynamicJsonDocument root(250);
      deserializeJson(root, (char*) data.c_str());
      cpuName = root["CPU"]["Name"].as<String>();
      cpuLoad = root["CPU"]["Load"].as<String>();
      cpuTemp = root["CPU"]["Temp"].as<String>();
      ramLoad = root["RAM"].as<String>();
      gpuName = root["GPU"]["Name"].as<String>();
      gpuLoad = root["GPU"]["Load"].as<String>();
      gpuTemp = root["GPU"]["Temp"].as<String>();
      if(cpuLoad.length()> 5)
      cpuLoad = cpuLoad.substring(0,5);
      if(lanDauMoApp) {
        border();
        lanDauMoApp = false;
      }
      else {
        if(oldCPULoad != cpuLoad) {
          tft.drawGFXText(3, 49, oldCPULoad+"%", 0x0000);
          tft.drawGFXText(3, 49, cpuLoad+"%", cpuColor);
          oldCPULoad = cpuLoad;
        }
        if(oldCPUTemp != cpuTemp) {
          hienThiNhietDo(30, 75, oldCPUTemp, 0x0000);
          hienThiNhietDo(30, 75, cpuTemp, cpuColor);
          oldCPUTemp = cpuTemp;
        }
        
        if(oldRAMLoad != ramLoad) {
          tft.drawGFXText(60, 126, oldRAMLoad+"%", 0x0000);
          tft.drawGFXText(60, 126, ramLoad+"%", ram);
          oldRAMLoad = ramLoad;
        }
      
        if(oldGPULoad != gpuLoad) {
          tft.drawGFXText(110, 49, oldGPULoad+"%", 0x0000);
          tft.drawGFXText(110, 49, gpuLoad+"%", gpuColor);
          oldGPULoad = gpuLoad;
        }
        if(oldGPUTemp != gpuTemp) {
          hienThiNhietDo(100, 75, oldGPUTemp, 0x0000);
          hienThiNhietDo(100, 75, gpuTemp, gpuColor);
          oldGPUTemp = gpuTemp;
        }
      }
    }
  }
  else {
    tft.clear();
    tft.setGFXFont(&FreeSans8pt7b);
    ketNoiApp = false;
  }
}
