@echo 
ffmpeg.exe -loglevel panic -i %1 -vn -acodec libopus -b:a 16k %2