# Hantek-UI
This code was developed for a Hantek Digital Multi-meter HDM3065 (3000 series).  
It uses SCPI commands to control the device and read its output.  
Since SCPI is supported by many multi-meters and the HDM3065 appears to be a Keysight clone it should be easy to fork the code and modify it to suit other multi-meters.  
It uses the NI VISA drivers to access the device through the USB port.
The drivers and other information is available at:  

Keysight I/O Bundle Drivers: https://www.keysight.com/us/en/lib/software-detail/computer-software/io-libraries-suite-downloads-2175637.html  

Hantek Website (Downloads): https://hantek.com/download?key=fjzl&sid=3018&pid=16176&word=  

NI Visa Documentation: https://www.ni.com/docs/en-US/bundle/ni-visa-api-ref/page/ni-visa-api-ref/ni-visa-api-ref.html