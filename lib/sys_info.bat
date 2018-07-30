@echo off
echo Machine Name:
hostname

echo Machine key:
wmic bios get serialnumber

pause