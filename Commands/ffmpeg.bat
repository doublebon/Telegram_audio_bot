@echo 
ffmpeg.exe -loglevel panic -i %1 -c:a libopus -compression_level 10 -frame_duration 60 -vbr on -application voip %2 -y