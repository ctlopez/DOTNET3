echo off

rem batch file to run a script to create a db
rem 11/1/2016

rem sqlcmd -S localhost\sqlexpress -E -i boatDB.sql
sqlcmd -S localhost\SQLCLOPEZ -E -i eventDB.sql

ECHO if no error messages appear DB was created
Pause