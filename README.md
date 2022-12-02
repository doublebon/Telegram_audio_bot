# Telegram_audio_bot
Bot that can send preloaded audio files as voice!

## Requirements
- `Docker` on unix

## Deploy and start on unix server
1. Unpack `telegram_audio_bot_*.zip`(from release page) at server folder
2. Edit at `appsettings.json` file fields: `TelegramBotToken` and `AdminUsernames` (yours username or anyone else)
3. Use `cd *path*` command at server to open folder with bot and enter commands: <br>`chmod +x bot_run_with_prune.sh` <br>`chmod +x bot_run.sh`
4. Start bot! 
<br>- If u want to start bot container with cleanup all old containers use `bot_run_with_prune.sh`(For your own risk) and print `Y` when requested
<br>- If u don't care about cleanup images or containers use `bot_run.sh`(Safe)
5. Now go to the chat with this bot and enter some text. Bot will reply

## Usage
Enter `@YourBotName` + hit space at any chat for send prewious added audio!

## How add new voice (admin only)
1. Send some voice file to bot chat (mp3 file needs to be converted to voice)
2. Bot sends u back `voiceFileId`
3. Send `/addvoice` in chat with bot
4. Then send on reply message text with format ` *title_name_in_inline*:*voiceFileId* `
5. If bot sends to u message about that new voice was added - all cool
6. Now, send `/upd` command to update voices list
7. Thats all, voices was added to inline query list

## For remove audios
1. Send `/delvoice` to bot chat
2. Send on reply message `*title_name_in_inline*` of prewious added voice file
