ʹ�÷�����
1.����OutlinePackage
2.�����������ӽű�Outline/Scripts/outlineCamera(��Ҫ��������Camera���)
3.��Outline/Shaders/Ŀ¼�µ�rtShader��displayShader�ϸ�outlineCamera�ϵĶ�Ӧ����
4.��������Ҫ��ߵ���������ӽű�Outline/Scripts/outlineDrawer(��Ҫ��������renderer)
6.����outlineDrawer�İ�Χ��ʹ���ܹ���Χס���������Ҿ���С
5.����״̬�¿ɿ������Ч��

����˵����
outlineCamera.cs��
Outline Scale: ��߿��
Outline Color: �����ɫ
Margin: RenderTexture����������֮�����������������ؿ��
Rt Width, Rt Height: ������RenderTexture�����ؿ��,��Ҫ������ʱ�޸�
Display Pos, Display Dir: ����������ڵ�����Ĳ���,һ�����ʹ������������

outlineDrawer.cs:
BdBox Center, BdBox Size: ��Χ�����ĺʹ�С

ʹ��ע�����
1.��������Ҫ�ֶ�Ϊÿ��������������С���ʵİ�Χ����ȷ����Ļ��Χ;
2.��Ҫ�������������̫��������̫��,���Ǵ�������İ�Χ��,�ⶼ�ᵼ����������Ļ��ռ�ݵķ�Χ����,ռ����Ļ��Χ����render texture�ֱ��ʵ�������߲��ᱻ����;
3.��������������/����ʱ,��ߵ��ڵ���ϵ���ܻ᲻����;
4.������Ҫ��ߵ������С���������þ���С��Rt Width, Rt Height�Խ��Ϳ���