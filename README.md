Unity Debug Logger
한국어 / English

![Output Example](result.jpg)

Repository: https://github.com/kimbabmon9356/unity-debug-logger.git

🌐 한국어

📢 소개
Unity 콘솔 로그를 채널 기반으로 보기 좋게 출력하기 위한 경량 디버그 로거입니다.
채널별 이름/색상/이모지를 설정할 수 있고, 로그 앞에 접두/접미를 붙여 구분성을 높일 수 있습니다.

이 레포지토리는 다음을 자동화합니다.
- LogChannelAsset 생성 (없을 경우 자동 생성)
- Channel enum 자동 생성 (채널 저장 시 자동 갱신)
- ENABLE_GAME_LOG 심볼 자동 관리 (개발 빌드에서 활성화)

🚧 설치
1. 이 레포지토리를 클론합니다.
2. Script 폴더를 Unity 프로젝트의 Assets 아래로 복사합니다.
3. Unity로 돌아오면 LogChannelAsset이 자동 생성됩니다.
4. LogChannelAsset에서 채널 목록을 수정하고 저장(Ctrl+S)하면 Channel enum이 자동 갱신됩니다.

📜 포함된 스크립트
- Console.cs : 채널 기반 로그 출력 API
- LogFormatter.cs : 채널 prefix/suffix, 색상, 이모지 포맷팅
- LogEntry.cs : 로그 데이터 구조체
- LogChannelAsset.cs : 채널 설정 ScriptableObject
- LogChannelConfig.cs : 채널별 설정 데이터
- Channel.cs : 자동 생성 채널 enum

Editor
- LogChannelAssetAutoCreator.cs : LogChannelAsset 자동 생성
- LogChannelGenerator.cs : Channel enum 자동 생성
- LogChannelConfigDrawer.cs : 채널 설정 커스텀 드로어(이모지 선택 포함)
- LogEmojiPickerPopup.cs / EmojiDatabaseBuilder.cs : 이모지 선택기 지원
- LoggerSymbolSetup.cs : ENABLE_GAME_LOG 심볼 자동 설정

🚀 간단 사용법
1) 기본 로그
```csharp
using UnityEngine;

Console.Log("Runtime Start");
Console.LogWarning("Something looks suspicious");
Console.LogError("Something went wrong");
```

2) 채널 로그
```csharp
using UnityEngine;

Console.Log(Channel.Character, "Status Initialized");
Console.Log(Channel.Monster, "Status Initialized");
Console.Log(Channel.Map, "Collider Initialized", "#64D46A");
```

3) 예외 로그
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

🖼️ 출력 예시
아래 이미지는 실제 콘솔 출력 예시입니다.

![Unity Debug Logger Example](result.jpg)

⚙️ 참고사항
- 대부분의 로그 메서드는 ENABLE_GAME_LOG 심볼이 있을 때만 출력됩니다.
- 일반 빌드에서는 심볼이 제거되어 로그 호출이 제외될 수 있습니다.
- 채널 이름은 enum 생성을 위해 영문/숫자/언더스코어(_) 사용을 권장합니다.
- Channel.cs는 자동 생성 파일이므로 수동 수정하지 않는 것을 권장합니다.
- README 인코딩은 UTF-8 기준입니다.

---

🌐 English

📢 Overview
A lightweight Unity debug logger that formats console output by channel.
You can configure channel name/color/emoji and add a customizable prefix/suffix for clearer logs.

This repository also automates:
- Auto-creation of LogChannelAsset (if missing)
- Auto-generation of Channel enum (on asset save)
- Auto-management of ENABLE_GAME_LOG define symbol (enabled for development)

🚧 Installation
1. Clone this repository.
2. Copy the Script folder under your Unity project's Assets.
3. Return to Unity and LogChannelAsset will be auto-created.
4. Edit channel entries in LogChannelAsset and save (Ctrl+S) to refresh Channel enum automatically.

📜 Included Scripts
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

🚀 Quick Usage
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

🖼️ Output Example
The image below shows a sample console output:

![Unity Debug Logger Example](result.jpg)

⚙️ Notes
- Most logging methods are compiled only when ENABLE_GAME_LOG is defined.
- In non-development builds, the symbol may be removed and log calls can be excluded.
- Use alphanumeric + underscore channel names for safe enum generation.
- Channel.cs is auto-generated, so avoid manual edits.
- README encoding is UTF-8.
