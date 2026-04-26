# 東吳校徽防衛戰 — AR Foundation 期末專案

> Unity ARFoundation 擴增實境塔防遊戲 | 東吳大學 2025 期末作品

---

## 目錄

- [專案簡介](#專案簡介)
- [遊戲玩法](#遊戲玩法)
- [系統需求](#系統需求)
- [安裝與建置](#安裝與建置)
- [操作說明](#操作說明)
- [專案結構](#專案結構)
- [腳本說明](#腳本說明)
- [預置物說明](#預置物說明)
- [套件相依性](#套件相依性)
- [開發備註](#開發備註)

---

## 專案簡介

本專案為東吳大學 AR 應用課程期末作品，利用 **Unity ARFoundation + ARCore** 在 Android 裝置上實現擴增實境遊戲。

玩家透過手機鏡頭掃描現實地板，將東吳校徽放置於真實空間中，並點擊消滅持續湧入的石頭人敵人，阻止牠們衝撞校徽。校徽血量歸零後遊戲結束。

**核心技術：**
- Unity 6 (URP 渲染管線)
- AR Foundation 6.0.6 + ARCore 6.0.6
- C# / TextMesh Pro / Particle System

---

## 遊戲玩法

```
[掃描地板] → [點擊放置校徽] → [點擊消滅石頭人] → [保護校徽存活]
```

| 事件 | 說明 |
|------|------|
| 放置校徽 | 點擊 AR 偵測到的地板平面，生成東吳校徽 |
| 敵人生成 | 石頭人每 3 秒從隨機生成點出現，自動朝校徽移動 |
| 消滅敵人 | 點擊石頭人即觸發爆炸特效並獲得 1 分 |
| 扣血機制 | 石頭人碰觸校徽觸發 Trigger，扣除 10 點血量後自爆 |
| 遊戲結束 | 校徽血量歸零，播放爆炸音效與特效，顯示 Game Over 畫面 |
| 重新開始 | 點擊 Retry 按鈕重新載入場景 |

---

## 系統需求

### 開發環境

| 項目 | 需求 |
|------|------|
| Unity 版本 | 6000.x (LTS 建議) |
| 渲染管線 | Universal Render Pipeline (URP) 17.0.3 |
| 平台模組 | Android Build Support |
| 作業系統 | Windows 10/11 或 macOS |

### 目標裝置

| 項目 | 需求 |
|------|------|
| 作業系統 | Android 7.0 (Nougat) 以上 |
| AR 支援 | ARCore 相容裝置 |
| 相機 | 後置鏡頭 |

> 確認裝置是否支援 ARCore：[https://developers.google.com/ar/devices](https://developers.google.com/ar/devices)

---

## 安裝與建置

### 1. 開啟專案

```bash
git clone https://github.com/Yuchen1222/Unity_SCU_ARFoundation_Final.git
# 使用 Unity Hub 開啟 Unity_SCU_ARFoundation_Final 資料夾
```

### 2. 確認套件已安裝

在 Unity 開啟後，Package Manager 會自動安裝 `manifest.json` 中的所有相依套件，包含：
- AR Foundation 6.0.6
- ARCore XR Plugin 6.0.6
- Universal RP 17.0.3

### 3. 切換建置平台

```
File → Build Settings → 選擇 Android → Switch Platform
```

### 4. 設定 Player Settings

```
Edit → Project Settings → Player → Android
  ✅ Minimum API Level: Android 7.0
  ✅ Scripting Backend: IL2CPP
  ✅ Target Architectures: ARM64
```

### 5. 建置並部署

```
File → Build Settings → Build And Run
```
將 Android 裝置透過 USB 連接（開啟開發者模式與 USB 偵錯），點擊 Build And Run 即可部署至裝置。

---

## 操作說明

| 動作 | 操作方式 |
|------|---------|
| 放置校徽 | 對準地板，點擊螢幕（觸控或滑鼠左鍵） |
| 消滅敵人 | 點擊場景中的石頭人 |
| 重新開始 | Game Over 畫面出現後點擊 Retry 按鈕 |

> 放置校徽後即無法移動，請選擇好位置後再點擊。

---

## 專案結構

```
Unity_SCU_ARFoundation_Final/
├── Assets/
│   ├── Scenes/
│   │   └── SampleScene.unity       # 主場景（唯一場景）
│   ├── Scripts/                    # 所有 C# 腳本
│   │   ├── ARManager.cs
│   │   ├── EmblemSpawner.cs
│   │   ├── EnemySpawner.cs
│   │   ├── EnemyAI.cs
│   │   ├── EmblemHealth.cs
│   │   ├── TapToDestroy.cs
│   │   └── RestartGame.cs
│   ├── 預置物/                      # Prefabs
│   ├── 音效/                        # Audio clips
│   ├── 圖片/                        # UI sprites / textures
│   ├── 動畫/                        # Animation controllers
│   ├── 材質球/                      # Materials
│   ├── 特效/                        # Particle effects (CFXR, Kino Bloom)
│   ├── 石頭人/                      # 石頭人敵人模型資源
│   ├── word/                       # 字型資源
│   ├── TextMesh Pro/               # TMP 字型與範例資源
│   ├── Settings/                   # URP 渲染設定
│   └── XR/                         # AR/XR 設定檔
├── Packages/
│   └── manifest.json               # 套件清單
└── ProjectSettings/                # Unity 專案設定
```

---

## 腳本說明

所有腳本皆位於 `Assets/Scripts/`，命名空間統一為 `namespace YUCHEN`。

---

### ARManager.cs

AR 場景管理器，負責在點擊時對 AR 偵測平面執行光線投射，並在命中位置實例化遊戲主體預置物（含生成點、UI 等完整遊戲結構）。負責放置整體遊戲場景預置物，與 `EmblemSpawner` 各司其職：本腳本放置遊戲主體，`EmblemSpawner` 負責放置校徽本身。

**掛載對象：** AR Session Origin（含 ARRaycastManager 元件的 GameObject）

| 欄位 | 類型 | 說明 |
|------|------|------|
| `goTD` | `GameObject` | 點擊地板後要生成的預置物（遊戲主體） |

**運作流程：**
1. `Awake()`：取得同一 GameObject 上的 `ARRaycastManager`
2. `Update()`：偵測到滑鼠左鍵按下時，向 AR 偵測平面發射光線
3. 命中後 Instantiate 預置物，設 `isPlaced = true` 防止重複放置

---

### EmblemSpawner.cs

校徽生成器，使用觸控輸入對 `PlaneWithinPolygon` 發射 AR 光線，首次命中後在命中位置生成校徽，並標記 Tag 為 `"Emblem"`。

**掛載對象：** AR Session Origin

| 欄位 | 類型 | 說明 |
|------|------|------|
| `emblemPrefab` | `GameObject` | 東吳校徽預置物 |

**運作流程：**
1. 偵測第一次觸控（`TouchPhase.Began`）
2. `ARRaycastManager.Raycast()` 命中平面後，Instantiate 校徽
3. `placedEmblem != null` 後不再生成（防止重複）
4. 生成後自動設定 Tag `"Emblem"`（供 `EnemyAI` 尋找目標）

---

### EnemySpawner.cs

敵人生成器，遊戲開始後每隔固定秒數從隨機生成點實例化一個石頭人。

**掛載對象：** 場景中任意空 GameObject（生成管理器）

| 欄位 | 類型 | 預設值 | 說明 |
|------|------|--------|------|
| `enemyPrefab` | `GameObject` | — | 石頭人敵人預置物 |
| `spawnPoints` | `Transform[]` | — | 生成點座標陣列 |
| `spawnInterval` | `float` | `3` | 生成間隔（秒） |

**運作流程：**
1. `Start()`：呼叫 `InvokeRepeating()`，延遲 1 秒後每隔 `spawnInterval` 秒執行一次 `SpawnEnemy()`
2. `SpawnEnemy()`：從 `spawnPoints` 隨機挑選一個位置，Instantiate 敵人

---

### EnemyAI.cs

石頭人 AI，控制敵人追蹤校徽、播放移動動畫，並在碰撞校徽後扣血自爆。

**掛載對象：** 石頭人敵人 Prefab

| 欄位 | 類型 | 預設值 | 說明 |
|------|------|--------|------|
| `speed` | `float` | `1.5` | 移動速度（Unity 單位/秒） |

**運作流程：**
1. `Start()`：用 `GameObject.FindWithTag("Emblem")` 取得目標；啟動 Animator Bool 參數 `"移動"`
2. `Update()`：計算朝校徽方向的單位向量（固定 Y=0 防止浮空），每幀移動並 `LookAt` 校徽
3. `OnTriggerEnter()`：偵測到 Tag `"Emblem"` 的 Collider 時，呼叫 `EmblemHealth.TakeDamage(10)` 後自我銷毀

> **注意：** 石頭人 Prefab 需掛載 **Trigger Collider**，校徽也需掛載 Collider（非 Trigger）。

---

### EmblemHealth.cs

校徽血量管理，處理扣血、UI 血條更新、爆炸特效、音效播放與 Game Over 觸發。

**掛載對象：** 東吳校徽 Prefab

| 欄位 | 類型 | 預設值 | 說明 |
|------|------|--------|------|
| `maxHealth` | `int` | `100` | 校徽最大血量 |
| `healthImage` | `Image` | — | 血條 UI Image（Filled 模式） |
| `explosionEffect` | `GameObject` | — | 校徽被摧毀時的爆炸特效 Prefab |
| `gameOverUI` | `GameObject` | — | Game Over 畫面 GameObject |
| `destroySound` | `AudioClip` | — | 被摧毀時播放的音效 |

**公開方法：**

```csharp
public void TakeDamage(int damage)
```

每次呼叫時：
1. 扣除 `damage` 點血量，下限為 0
2. 更新血條 `fillAmount = currentHealth / maxHealth`
3. 血量歸零時：播放音效 → 生成爆炸特效 → 顯示 Game Over UI → 銷毀校徽

> `healthImage` 若未在 Inspector 指定，會在 `Start()` 自動以 `GameObject.Find("圖片_血條")` 尋找。

---

### TapToDestroy.cs

點擊消滅系統，對玩家點擊位置發射射線，命中 Tag 為 `"Enemy"` 的敵人時播放爆炸特效、銷毀敵人並更新分數。

**掛載對象：** 場景中的遊戲管理器（或直接掛在 Camera）

| 欄位 | 類型 | 說明 |
|------|------|------|
| `explosionEffectPrefab` | `GameObject` | 擊殺敵人的爆炸特效 Prefab |
| `scoreText` | `TextMeshProUGUI` | 顯示分數的 TMP 文字元件 |

**運作流程：**
1. `Update()`：偵測到左鍵按下時，從主相機發射 `Physics.Raycast()`
2. 命中 Tag `"Enemy"` 的物件時：在敵人位置 Instantiate 爆炸特效（2 秒後自動銷毀）
3. 銷毀敵人，`score += 1`，更新 UI 顯示 `"Score: N"`

---

### RestartGame.cs

場景重啟工具，提供公開方法供 UI 按鈕呼叫。

**掛載對象：** Retry 按鈕或任意管理器 GameObject

**公開方法：**

```csharp
public void RestartScene()
```

呼叫 `SceneManager.LoadScene(SceneManager.GetActiveScene().name)` 重新載入當前場景，重置所有遊戲狀態。

> 在 Inspector 中將 Retry 按鈕的 `OnClick()` 事件指向此方法即可。

---

## 預置物說明

位於 `Assets/預置物/`

| 預置物 | 說明 |
|--------|------|
| `SoochowEmblem.prefab` | 玩家防守的東吳校徽，掛載 `EmblemHealth`，需有 Collider |
| `石頭人.prefab` | 敵人，掛載 `EnemyAI`，需有 Trigger Collider 與 Animator |
| `東吳校徽防衛戰.prefab` | 遊戲場景完整預置物（含生成點、UI 等子物件） |
| `擴增實境地板.prefab` | AR 平面偵測視覺化標記 |
| `小爆炸.prefab` | 小型爆炸粒子特效 |
| `兩段爆炸.prefab` | 兩段式爆炸粒子特效 |
| `雷電爆炸.prefab` | 雷電爆炸粒子特效 |
| `圓環法陣特效.prefab` | 圓環魔法陣粒子特效 |

---

## 套件相依性

| 套件 | 版本 | 用途 |
|------|------|------|
| `com.unity.xr.arfoundation` | 6.0.6 | AR 核心框架（平面偵測、光線投射） |
| `com.unity.xr.arcore` | 6.0.6 | Android ARCore 後端 |
| `com.unity.render-pipelines.universal` | 17.0.3 | 行動裝置最佳化渲染管線 |
| `com.unity.inputsystem` | 1.11.2 | 新版輸入系統 |
| `com.unity.ugui` | 2.0.0 | Canvas / Image / Button UI 系統 |
| `com.unity.ai.navigation` | 2.0.5 | AI 導航（NavMesh，預留擴充） |
| `com.unity.timeline` | 1.8.7 | 時間軸動畫系統 |
| `com.unity.visualscripting` | 1.9.5 | 視覺化腳本 |
| `com.unity.collab-proxy` | 2.8.2 | Unity Version Control |

---

## 開發備註

### GameObject Tag 對照

| Tag | 物件 | 使用腳本 |
|-----|------|---------|
| `"Emblem"` | 東吳校徽 | `EnemyAI`（尋找目標）、`EnemyAI.OnTriggerEnter`（判斷碰撞） |
| `"Enemy"` | 石頭人敵人 | `TapToDestroy`（判斷點擊目標） |

### 重要 GameObject 名稱

| 名稱 | 說明 |
|------|------|
| `"圖片_血條"` | 血條 Image UI，`EmblemHealth` 未在 Inspector 設定時自動尋找 |

### Animator 參數

| 參數名稱 | 類型 | 說明 |
|----------|------|------|
| `"移動"` | Bool | 石頭人移動動畫觸發，於 `EnemyAI.Start()` 設為 `true` |

### 命名空間

所有腳本皆使用 `namespace YUCHEN { }` 封裝，避免與其他 Unity 套件命名衝突。

### 已知限制

- `EnemyAI` 在 `Start()` 尋找校徽：若校徽尚未放置，敵人將靜止不動（需確保校徽先放置或延遲生成敵人）
- `TapToDestroy` 與 `EmblemSpawner` 共用左鍵/觸控事件，點擊放置校徽時可能同時觸發射線判定
- 場景僅有一個（`SampleScene`），重啟遊戲以重新載入場景方式實現

---

*東吳大學 ARFoundation 期末專案 — 2025.06.15*
