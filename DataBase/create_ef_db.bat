echo off

rem batch file to run a script to create a db
rem 2017/05/03

sqlcmd -S localhost -E -i ASPIdentityData.sql

ECHO if no error messages appear DB was created
Pause