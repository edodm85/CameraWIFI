# Camera WIFI

![](Resources/ic_launcher.png)

<br>

## What is Camera WIFI?

Camera WIFI is not a traditional Camera App but it’s something different.
I writed this Application to create a stream of images between Phone and PC.

You can download from Google Play here:
* [Camera WIFI FREE (with ADS)](https://play.google.com/store/apps/details?id=com.edodm85.cameratcp.free)
* [Camera WIFI (without ADS)](https://play.google.com/store/apps/details?id=com.edodm85.cameratcp.paid)

<br>

## How does it work?

1. Connect Phone and PC over the same network

2. Open the Application Camera WIFI on phone

![](Resources/Screen1%20CameraWIFI.png)

3. Open the PC client PhoneTCPClient

4. Insert the ip address of the Phone and press Connect

![](Resources/Screen3%20PhoneTCPClient.PNG)

5. Press snap button and acquire the image

![](Resources/Screen4%20PhoneTCPClient.PNG)

<br>

## Building PhoneTCPClient

For build [PhoneTCPClient](https://github.com/edodm85/CameraWIFI/tree/master/PhoneTCPClient%20Source%20Code) you need Visual Studio 2013 or above.

<br>

## Create your Client (Camera WIFI v3.0.0 or higher)

The protocol description is here: https://github.com/edodm85/DOT_Protocol_Specification

#### COMMANDS FOR RECEIVED IMAGES

1. START ACQUISITION: Send this array: [0xAA 0x1 0x0 0x0 0x0 0x3 0x1 0x11 0x55]

2. STOP ACQUISITION: Send this array: [0xAA 0x1 0x0 0x0 0x0 0x3 0x1 0x12 0x55]

3. ACQUIRE ONE IMAGE: Send this array: [0xAA 0x1 0x0 0x0 0x0 0x3 0x1 0x10 0x55]

#### RECEIVE IMAGES FROM CAMERA WIFI

The App sends an image, with this Bytes sequence: [0xAA 0x1 LENGHT 0x5 PAYLOAD 0x55]



<br>

<br>

## (DEPRECATED) Create your Client (All versions)

Camera WIFI accepts these commands via wifi:

#### COMMANDS FOR RECEIVED IMAGES

1. START ACQUISITION: Send the string "startGRAB"

2. STOP ACQUISITION: Send the string "stopGRAB"

3. ACQUIRE ONE IMAGE: Send the string "singleSNAP"

#### RECEIVE IMAGES FROM CAMERA WIFI

The App sends an image, in the first place it adds the string "sRt" and in the end the string "sTp".

So the client receives: "sRt" - Bytes image - "sTp"


#### COMMANDS FOR ENABLE/DISABLE SOME FUNCTIONS

1. ENABLE FLASH: Send the string "flashON"

2. DISABLE FLASH: Send the string "flashOFF"
          
3. ENABLE AUTOFOCUS: Send the string "focusON"

4. DISABLE AUTOFOCUS: Send the string "focusOFF"  

<br>

## License

> Copyright (C) 2018 edodm85.  
> Licensed under the MIT license.  
> (See the [LICENSE](https://github.com/edodm85/CameraWIFI/blob/master/LICENSE) file for the whole license text.)
