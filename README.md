# Telegram_audio_bot

For start enter your telegram bot token and admin nickname (yours probably) into appsettings.json

## Usage
Enter @YourBotName at any chat for send prewious added audio!

## How add new voice
1. Send some voice file to bot chat (mp3 file needs to be converted to voice)
2. Bot sends u back "voiceFileId"
3. Send ' /addvoice ' in chat with bot
4. Then send on reply message text with format ' title_name_in_inline:voiceFileId '
5. If bot sends to u message about that new voice was added - all cool

## For remove audios
1. Send ' /delvoice ' to bot chat
2. Send on reply message voice title name on inline
