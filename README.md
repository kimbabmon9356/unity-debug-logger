Unity Debug Logger
?쒓뎅??/ English

![Output Example](result.jpg)

Repository: https://github.com/kimbabmon9356/unity-debug-logger.git

?뙋 ?쒓뎅??

?뱼 ?뚭컻
Unity 肄섏넄 濡쒓렇瑜?梨꾨꼸 湲곕컲?쇰줈 蹂닿린 醫뗪쾶 異쒕젰?섍린 ?꾪븳 寃쎈웾 ?붾쾭洹?濡쒓굅?낅땲??
梨꾨꼸蹂??대쫫/?됱긽/?대え吏瑜??ㅼ젙?????덇퀬, 濡쒓렇 ?욎뿉 ?묐몢/?묐?瑜?遺숈뿬 援щ텇?깆쓣 ?믪씪 ???덉뒿?덈떎.

???덊룷吏?좊━???ㅼ쓬???먮룞?뷀빀?덈떎.
- LogChannelAsset ?앹꽦 (?놁쓣 寃쎌슦 ?먮룞 ?앹꽦)
- Channel enum ?먮룞 ?앹꽦 (梨꾨꼸 ??????먮룞 媛깆떊)
- ENABLE_GAME_LOG ?щ낵 ?먮룞 愿由?(媛쒕컻 鍮뚮뱶?먯꽌 ?쒖꽦??

?슙 ?ㅼ튂
1. ???덊룷吏?좊━瑜??대줎?⑸땲??
2. Script ?대뜑瑜?Unity ?꾨줈?앺듃??Assets ?꾨옒濡?蹂듭궗?⑸땲??
3. Unity濡??뚯븘?ㅻ㈃ LogChannelAsset???먮룞 ?앹꽦?⑸땲??
4. LogChannelAsset?먯꽌 梨꾨꼸 紐⑸줉???섏젙?섍퀬 ???Ctrl+S)?섎㈃ Channel enum???먮룞 媛깆떊?⑸땲??

?뱶 ?ы븿???ㅽ겕由쏀듃
- Console.cs : 梨꾨꼸 湲곕컲 濡쒓렇 異쒕젰 API
- LogFormatter.cs : 梨꾨꼸 prefix/suffix, ?됱긽, ?대え吏 ?щ㎎??
- LogEntry.cs : 濡쒓렇 ?곗씠??援ъ“泥?
- LogChannelAsset.cs : 梨꾨꼸 ?ㅼ젙 ScriptableObject
- LogChannelConfig.cs : 梨꾨꼸蹂??ㅼ젙 ?곗씠??
- Channel.cs : ?먮룞 ?앹꽦 梨꾨꼸 enum

Editor
- LogChannelAssetAutoCreator.cs : LogChannelAsset ?먮룞 ?앹꽦
- LogChannelGenerator.cs : Channel enum ?먮룞 ?앹꽦
- LogChannelConfigDrawer.cs : 梨꾨꼸 ?ㅼ젙 而ㅼ뒪? ?쒕줈???대え吏 ?좏깮 ?ы븿)
- LogEmojiPickerPopup.cs / EmojiDatabaseBuilder.cs : ?대え吏 ?좏깮湲?吏??
- LoggerSymbolSetup.cs : ENABLE_GAME_LOG ?щ낵 ?먮룞 ?ㅼ젙

?? 媛꾨떒 ?ъ슜踰?
1) 湲곕낯 濡쒓렇
```csharp
using UnityEngine;

Console.Log("Runtime Start");
Console.LogWarning("Something looks suspicious");
Console.LogError("Something went wrong");
```

2) 梨꾨꼸 濡쒓렇
```csharp
using UnityEngine;

Console.Log(Channel.Character, "Status Initialized");
Console.Log(Channel.Monster, "Status Initialized");
Console.Log(Channel.Map, "Collider Initialized", "#64D46A");
```

3) ?덉쇅 濡쒓렇
```csharp
try
{
    // ...
}
catch (System.Exception e)
{
    Console.LogException(e);
}
```

?뼹截?異쒕젰 ?덉떆
?꾨옒 ?대?吏???ㅼ젣 肄섏넄 異쒕젰 ?덉떆?낅땲??

![Unity Debug Logger Example](result.jpg)

?숋툘 李멸퀬?ы빆
- ?遺遺꾩쓽 濡쒓렇 硫붿꽌?쒕뒗 ENABLE_GAME_LOG ?щ낵???덉쓣 ?뚮쭔 異쒕젰?⑸땲??
- ?쇰컲 鍮뚮뱶?먯꽌???щ낵???쒓굅?섏뼱 濡쒓렇 ?몄텧???쒖쇅?????덉뒿?덈떎.
- 梨꾨꼸 ?대쫫? enum ?앹꽦???꾪빐 ?곷Ц/?レ옄/?몃뜑?ㅼ퐫??_) ?ъ슜??沅뚯옣?⑸땲??
- Channel.cs???먮룞 ?앹꽦 ?뚯씪?대?濡??섎룞 ?섏젙?섏? ?딅뒗 寃껋쓣 沅뚯옣?⑸땲??
- ?대떦 README.md ??AI濡??명빐 ?묒꽦?섏뿀?듬땲??

---

?뙋 English

?뱼 Overview
A lightweight Unity debug logger that formats console output by channel.
You can configure channel name/color/emoji and add a customizable prefix/suffix for clearer logs.

This repository also automates:
- Auto-creation of LogChannelAsset (if missing)
- Auto-generation of Channel enum (on asset save)
- Auto-management of ENABLE_GAME_LOG define symbol (enabled for development)

?슙 Installation
1. Clone this repository.
2. Copy the Script folder under your Unity project's Assets.
3. Return to Unity and LogChannelAsset will be auto-created.
4. Edit channel entries in LogChannelAsset and save (Ctrl+S) to refresh Channel enum automatically.

?뱶 Included Scripts
- Console.cs : Channel-based logging API
- LogFormatter.cs : Prefix/suffix, color, emoji formatting
- LogEntry.cs : Log data struct
- LogChannelAsset.cs : Channel configuration ScriptableObject
- LogChannelConfig.cs : Per-channel config model
- Channel.cs : Auto-generated channel enum

Editor
- LogChannelAssetAutoCreator.cs : Auto-creates LogChannelAsset
- LogChannelGenerator.cs : Auto-generates Channel enum
- LogChannelConfigDrawer.cs : Custom channel drawer (with emoji picker)
- LogEmojiPickerPopup.cs / EmojiDatabaseBuilder.cs : Emoji picker support
- LoggerSymbolSetup.cs : Auto-sets ENABLE_GAME_LOG define symbol

?? Quick Usage
1) Basic logging
```csharp
using UnityEngine;

Console.Log("Runtime Start");
Console.LogWarning("Something looks suspicious");
Console.LogError("Something went wrong");
```

2) Channel logging
```csharp
using UnityEngine;

Console.Log(Channel.Character, "Status Initialized");
Console.Log(Channel.Monster, "Status Initialized");
Console.Log(Channel.Map, "Collider Initialized", "#64D46A");
```

3) Exception logging
```csharp
try
{
    // ...
}
catch (System.Exception e)
{
    Console.LogException(e);
}
```

?뼹截?Output Example
The image below shows a sample console output:

![Unity Debug Logger Example](result.jpg)

?숋툘 Notes
- Most logging methods are compiled only when ENABLE_GAME_LOG is defined.
- In non-development builds, the symbol may be removed and log calls can be excluded.
- Use alphanumeric + underscore channel names for safe enum generation.
- Channel.cs is auto-generated, so avoid manual edits.
