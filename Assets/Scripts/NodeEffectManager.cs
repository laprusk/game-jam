
using UnityEngine;

public class NodeEffectManager
{
    private struct NodeEffect
    {
        public string message;
        public int minRange;
        public int maxRange;
    }

    private static NodeEffect[] increaseSupportersEffects = new NodeEffect[]
    {
        // 微増
        new NodeEffect { message = "地域コミュニティの掲示板に名前が載り、数人が興味を持った。", minRange = 2, maxRange = 10 },
        new NodeEffect { message = "まずまずの街頭演説を行うことができた。", minRange = 11, maxRange = 50 },
        new NodeEffect { message = "挨拶回り中に、数人の住民と笑顔で会話できた。", minRange = 10, maxRange = 21 },
        // 一般
        new NodeEffect { message = "街頭演説に確かな手応えがあった。", minRange = 100, maxRange = 201 },
        new NodeEffect { message = "ボランティア活動が功を奏し、地域住民の信頼を獲得した。", minRange = 200, maxRange = 301 },
        new NodeEffect { message = "オンライン討論会で他の候補者を圧倒し、高評価を得た。", minRange = 300, maxRange = 401 },
        new NodeEffect { message = "政策提案がメディアで取り上げられ、評判が広がった。", minRange = 200, maxRange = 1001 },
        new NodeEffect { message = "対立候補の問題発言が話題となり、その反動で支持者が流れ込んできた。", minRange = 300, maxRange = 1001 },
        // 急増
        new NodeEffect { message = "街頭演説で多くの聴衆の心を動かした！", minRange = 100, maxRange = 501 },
        new NodeEffect { message = "SNSで多くのユーザーに政策を支持する声が広がった！", minRange = 200, maxRange = 1001 },

    };

    private static NodeEffect[] decreaseSupportersEffects = new NodeEffect[]
    {
        // 微減
        new NodeEffect { message = "街頭演説に人が集まらなかった……。悲しい！", minRange = 0, maxRange = 1 },
        new NodeEffect { message = "挨拶回り中に、住民に無視されてしまった。", minRange = 0, maxRange = 1 },
        // 一般
        new NodeEffect { message = "街頭演説中に失言し、その場で批判を浴びた。", minRange = 100, maxRange = 201 },
        new NodeEffect { message = "地域新聞で自身の政策が批判されてしまった。", minRange = 200, maxRange = 301 },
        new NodeEffect { message = "有権者からの質問に答えられず、「準備不足」と批判された。", minRange = 300, maxRange = 401 },
        new NodeEffect { message = "公約の実現性に疑問が呈され、専門家から批判を受けた。", minRange = 300, maxRange = 401 },
        new NodeEffect { message = "公開討論会で対立候補に言い負かされてしまった。", minRange = 200, maxRange = 1001 },
        // 急減
        new NodeEffect { message = "スキャンダルが発覚し、支持率が下がった。", minRange = 100, maxRange = 501 },
        new NodeEffect { message = "自身の発言がSNSで炎上してしまった。", minRange = 200, maxRange = 1001 },
    };

    public static (int, string) GetRandomIncreasingEffect()
    {
        NodeEffect effect = increaseSupportersEffects[Random.Range(0, increaseSupportersEffects.Length)];
        int supportersChange = Random.Range(effect.minRange, effect.maxRange + 1);
        return (supportersChange, effect.message);
    }

    public static (int, string) GetRandomDecreasingEffect()
    {
        NodeEffect effect = decreaseSupportersEffects[Random.Range(0, decreaseSupportersEffects.Length)];
        int supportersChange = Random.Range(effect.minRange, effect.maxRange + 1);
        return (supportersChange, effect.message);
    }
}