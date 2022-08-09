#!/bin/bash
docker build -t telbot/bot:1 . && docker run --name telbot -itd --rm telbot/bot:1; docker image prune