// よく入門書などで、「 インスペクターに変数の値を表示するには public にする」ということを目にしたか
// もしれません。これはプログラマー以外でも理解しやすいように言っているだけで、public 変数にするのは
// シリアライズ対象の条件の 1 つでしかありません。エディター拡張を行うユーザーは private フィールドに
// SerializeField 属性を付けることをお勧めします。
// [SerializeField]
// private string m_str;
// public string str
// {
//     get
//     {
//         return m_str;
//     }
//     set
//     {
//         m_str = value;
//     }
// }