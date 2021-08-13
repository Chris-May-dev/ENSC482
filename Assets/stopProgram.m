%timerStop.m

%Run this script to stop running the timer and shapePredict.m

myTimers = timerfind;
stop(myTimers);
delete(myTimers);