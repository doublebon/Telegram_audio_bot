#!/bin/bash
ffmpeg -loglevel panic -i $1 -vn -acodec libopus -b:a 16k $2