<map version="freeplane 1.7.0">
<!--To view this file, download free mind mapping software Freeplane from http://freeplane.sourceforge.net -->
<node TEXT="Cross Platform Haptics" FOLDED="false" ID="ID_1207000042" CREATED="1558021191772" MODIFIED="1558021205214" STYLE="oval">
<font SIZE="18"/>
<hook NAME="MapStyle">
    <properties fit_to_viewport="false" edgeColorConfiguration="#808080ff,#ff0000ff,#0000ffff,#00ff00ff,#ff00ffff,#00ffffff,#7c0000ff,#00007cff,#007c00ff,#7c007cff,#007c7cff,#7c7c00ff"/>

<map_styles>
<stylenode LOCALIZED_TEXT="styles.root_node" STYLE="oval" UNIFORM_SHAPE="true" VGAP_QUANTITY="24.0 pt">
<font SIZE="24"/>
<stylenode LOCALIZED_TEXT="styles.predefined" POSITION="right" STYLE="bubble">
<stylenode LOCALIZED_TEXT="default" ICON_SIZE="12.0 pt" COLOR="#000000" STYLE="fork">
<font NAME="SansSerif" SIZE="10" BOLD="false" ITALIC="false"/>
</stylenode>
<stylenode LOCALIZED_TEXT="defaultstyle.details"/>
<stylenode LOCALIZED_TEXT="defaultstyle.attributes">
<font SIZE="9"/>
</stylenode>
<stylenode LOCALIZED_TEXT="defaultstyle.note" COLOR="#000000" BACKGROUND_COLOR="#ffffff" TEXT_ALIGN="LEFT"/>
<stylenode LOCALIZED_TEXT="defaultstyle.floating">
<edge STYLE="hide_edge"/>
<cloud COLOR="#f0f0f0" SHAPE="ROUND_RECT"/>
</stylenode>
</stylenode>
<stylenode LOCALIZED_TEXT="styles.user-defined" POSITION="right" STYLE="bubble">
<stylenode LOCALIZED_TEXT="styles.topic" COLOR="#18898b" STYLE="fork">
<font NAME="Liberation Sans" SIZE="10" BOLD="true"/>
</stylenode>
<stylenode LOCALIZED_TEXT="styles.subtopic" COLOR="#cc3300" STYLE="fork">
<font NAME="Liberation Sans" SIZE="10" BOLD="true"/>
</stylenode>
<stylenode LOCALIZED_TEXT="styles.subsubtopic" COLOR="#669900">
<font NAME="Liberation Sans" SIZE="10" BOLD="true"/>
</stylenode>
<stylenode LOCALIZED_TEXT="styles.important">
<icon BUILTIN="yes"/>
</stylenode>
</stylenode>
<stylenode LOCALIZED_TEXT="styles.AutomaticLayout" POSITION="right" STYLE="bubble">
<stylenode LOCALIZED_TEXT="AutomaticLayout.level.root" COLOR="#000000" STYLE="oval" SHAPE_HORIZONTAL_MARGIN="10.0 pt" SHAPE_VERTICAL_MARGIN="10.0 pt">
<font SIZE="18"/>
</stylenode>
<stylenode LOCALIZED_TEXT="AutomaticLayout.level,1" COLOR="#0033ff">
<font SIZE="16"/>
</stylenode>
<stylenode LOCALIZED_TEXT="AutomaticLayout.level,2" COLOR="#00b439">
<font SIZE="14"/>
</stylenode>
<stylenode LOCALIZED_TEXT="AutomaticLayout.level,3" COLOR="#990000">
<font SIZE="12"/>
</stylenode>
<stylenode LOCALIZED_TEXT="AutomaticLayout.level,4" COLOR="#111111">
<font SIZE="10"/>
</stylenode>
<stylenode LOCALIZED_TEXT="AutomaticLayout.level,5"/>
<stylenode LOCALIZED_TEXT="AutomaticLayout.level,6"/>
<stylenode LOCALIZED_TEXT="AutomaticLayout.level,7"/>
<stylenode LOCALIZED_TEXT="AutomaticLayout.level,8"/>
<stylenode LOCALIZED_TEXT="AutomaticLayout.level,9"/>
<stylenode LOCALIZED_TEXT="AutomaticLayout.level,10"/>
<stylenode LOCALIZED_TEXT="AutomaticLayout.level,11"/>
</stylenode>
</stylenode>
</map_styles>
</hook>
<hook NAME="AutomaticEdgeColor" COUNTER="5" RULE="ON_BRANCH_CREATION"/>
<node TEXT="Haptics" POSITION="right" ID="ID_576858799" CREATED="1558034143194" MODIFIED="1558034145220">
<edge COLOR="#00ffff"/>
<node TEXT="iOS" FOLDED="true" ID="ID_221538373" CREATED="1558021205943" MODIFIED="1558034158532">
<node TEXT="docs" ID="ID_170922133" CREATED="1558031227574" MODIFIED="1558031239629" LINK="https://docs.microsoft.com/en-us/xamarin/ios/user-interface/ios-ui/haptic-feedback"/>
<node TEXT="UIImpactFeedbackGenerator" ID="ID_1343915113" CREATED="1558031293320" MODIFIED="1558031295459">
<node TEXT="using UIKit;&#xa;...&#xa;&#xa;// Initialize feedback&#xa;var impact = new UIImpactFeedbackGenerator (UIImpactFeedbackStyle.Heavy);&#xa;impact.Prepare ();&#xa;&#xa;// Trigger feedback&#xa;impact.ImpactOccurred ();" ID="ID_23603075" CREATED="1558031306663" MODIFIED="1558031307520"/>
<node TEXT="levels:" ID="ID_1705217686" CREATED="1558031309813" MODIFIED="1558031311906">
<node TEXT="Light" ID="ID_323049773" CREATED="1558031314875" MODIFIED="1558031318364"/>
<node TEXT="Medium" ID="ID_1476588369" CREATED="1558031318813" MODIFIED="1558031320615"/>
<node TEXT="Heavy" ID="ID_520310044" CREATED="1558031321018" MODIFIED="1558031323089"/>
</node>
</node>
<node TEXT="UINotificationFeedbackGenerator" ID="ID_658550583" CREATED="1558031260582" MODIFIED="1558031261887">
<node TEXT="using UIKit;&#xa;...&#xa;&#xa;// Initialize feedback&#xa;var notification = new UINotificationFeedbackGenerator ();&#xa;notification.Prepare ();&#xa;&#xa;// Trigger feedback&#xa;notification.NotificationOccurred (UINotificationFeedbackType.Error);" ID="ID_1541345571" CREATED="1558031267468" MODIFIED="1558031268685"/>
<node TEXT="levels:" ID="ID_632159524" CREATED="1558031275232" MODIFIED="1558031277819">
<node TEXT="Success" ID="ID_1154780418" CREATED="1558031278223" MODIFIED="1558031280091"/>
<node TEXT="Warning" ID="ID_185860532" CREATED="1558031280427" MODIFIED="1558031281666"/>
<node TEXT="Error" ID="ID_496490150" CREATED="1558031281890" MODIFIED="1558031282926"/>
</node>
</node>
<node TEXT="UISelectionFeedbackGenerator" ID="ID_1771207891" CREATED="1558031210160" MODIFIED="1558031222895">
<node TEXT="using UIKit;&#xa;...&#xa;&#xa;// Initialize feedback&#xa;var selection = new UISelectionFeedbackGenerator ();&#xa;selection.Prepare ();&#xa;&#xa;// Trigger feedback&#xa;selection.SelectionChanged ();" ID="ID_1102039913" CREATED="1558031248816" MODIFIED="1558031250526"/>
</node>
</node>
<node TEXT="Android" ID="ID_888667559" CREATED="1558021208689" MODIFIED="1558034158538">
<node TEXT="View.PerformHapticFeedback" ID="ID_1613645309" CREATED="1558097817532" MODIFIED="1558097828491">
<node TEXT="HapticFeedbackContants" ID="ID_87138326" CREATED="1558097888048" MODIFIED="1558097934265" LINK="https://developer.android.com/reference/android/view/HapticFeedbackConstants">
<node TEXT="CLOCK_TICK" ID="ID_181390329" CREATED="1558097944996" MODIFIED="1558097948328"/>
<node TEXT="CONTEXT_CLICK" ID="ID_1638324748" CREATED="1558097950374" MODIFIED="1558097953817"/>
<node TEXT="FLAG_IGNORE_GLOBAL_SETTING" ID="ID_1160201180" CREATED="1558097964751" MODIFIED="1558097977353"/>
<node TEXT="FLAG_IGNORE_VIEW_SETTING" ID="ID_1878409937" CREATED="1558097984620" MODIFIED="1558097989885"/>
<node TEXT="KEYBOARD_PRESS" ID="ID_538855186" CREATED="1558097991887" MODIFIED="1558097994926"/>
<node TEXT="KEYBOARD_RELEASE" ID="ID_758204954" CREATED="1558097996500" MODIFIED="1558097999718"/>
<node TEXT="KEYBOARD_TAP" ID="ID_903329665" CREATED="1558098002372" MODIFIED="1558098007234"/>
<node TEXT="LONG_PRESS" ID="ID_1777675609" CREATED="1558098008537" MODIFIED="1558098012251"/>
<node TEXT="TEXT_HANDLE_MOVE" ID="ID_693087312" CREATED="1558098014433" MODIFIED="1558098018798"/>
<node TEXT="VIRTUAL_KEY" ID="ID_687581624" CREATED="1558098024200" MODIFIED="1558098029014"/>
<node TEXT="VIRTUAL_KEY_RELEASE" ID="ID_1679716511" CREATED="1558098030161" MODIFIED="1558098036980"/>
</node>
</node>
<node TEXT="Vibration" ID="ID_384931107" CREATED="1558097843047" MODIFIED="1558097868586">
<node TEXT="article" ID="ID_489272647" CREATED="1558028354617" MODIFIED="1558028369464" LINK="https://proandroiddev.com/using-vibrate-in-android-b0e3ef5d5e07"/>
<node TEXT="Official docs" ID="ID_1113929678" CREATED="1558028753638" MODIFIED="1558028761717">
<node TEXT="Vibrator" FOLDED="true" ID="ID_1760255972" CREATED="1558028898675" MODIFIED="1558028906371">
<node TEXT="Depricated in API26" ID="ID_1125091952" CREATED="1558028984626" MODIFIED="1558028991152"/>
<node TEXT="Methods" ID="ID_1473265070" CREATED="1558029251525" MODIFIED="1558029255082">
<node TEXT="void Cancel()" ID="ID_714976773" CREATED="1558029319769" MODIFIED="1558029982315"/>
<node TEXT="bool HasAmplitudeControl()" ID="ID_740395038" CREATED="1558029957855" MODIFIED="1558029977455"/>
<node TEXT="bool HasVibrator()" ID="ID_498003963" CREATED="1558029964853" MODIFIED="1558029971763"/>
<node TEXT="void Vibrate(long milliseconds)" ID="ID_1762750410" CREATED="1558029985081" MODIFIED="1558030934567">
<icon BUILTIN="button_cancel"/>
</node>
<node TEXT="void Vibrate(VibrationEffect vibe)" ID="ID_407905863" CREATED="1558030000651" MODIFIED="1558030904154">
<font BOLD="true"/>
</node>
<node TEXT="void Vibrate(long[] pattern, int repeate)" ID="ID_1554203666" CREATED="1558030061199" MODIFIED="1558030943892">
<icon BUILTIN="button_cancel"/>
</node>
<node TEXT="void Vibrate(long[] pattern, int repeate, AudioAttributes attributes)" ID="ID_1129554220" CREATED="1558030077985" MODIFIED="1558030950653">
<icon BUILTIN="button_cancel"/>
</node>
<node TEXT="void Vibrate(VibrationEffect vibe, AudioAttributes attributes)" ID="ID_1974081428" CREATED="1558030117856" MODIFIED="1558030898577">
<font BOLD="true"/>
</node>
<node TEXT="void Vibrate(long milliseoncs, AudioAttributes attributes)" ID="ID_315351738" CREATED="1558030138399" MODIFIED="1558030971027">
<icon BUILTIN="button_cancel"/>
</node>
</node>
</node>
<node TEXT="AudioAttributes" ID="ID_1354199714" CREATED="1558030154374" MODIFIED="1558030606092" LINK="https://developer.android.com/reference/android/media/AudioAttributes.html">
<node TEXT="interesting constants" ID="ID_269018099" CREATED="1558030714589" MODIFIED="1558030720778">
<node TEXT="CONTENT_TYPE_SONIFICATION" ID="ID_1257907416" CREATED="1558030721114" MODIFIED="1558030730632"/>
<node TEXT="USAGE_ASSISTANCE_SONIFICATION" ID="ID_118175965" CREATED="1558030758464" MODIFIED="1558030863767">
<font BOLD="true"/>
</node>
<node TEXT="USAGE_NOTIFICATION" ID="ID_961551195" CREATED="1558030782474" MODIFIED="1558030787379"/>
<node TEXT="USAGE_NOTIFICATION_EVENT" ID="ID_1311475907" CREATED="1558030797660" MODIFIED="1558030806707"/>
<node TEXT="USAGE_UNKNOWN" ID="ID_8677706" CREATED="1558030810665" MODIFIED="1558030814604"/>
</node>
</node>
<node TEXT="VibrationEffect" ID="ID_1809268871" CREATED="1558028762323" MODIFIED="1558028775707" LINK="https://developer.android.com/reference/android/os/VibrationEffect.html">
<node TEXT="Added in API26" ID="ID_561249832" CREATED="1558028970294" MODIFIED="1558028977922"/>
<node TEXT="Constants" ID="ID_80757664" CREATED="1558028827979" MODIFIED="1558028835136">
<node TEXT="DEFAULT_AMPLITUDE" ID="ID_510697338" CREATED="1558028835426" MODIFIED="1558028841548"/>
<node TEXT="EFFECT_CLICK" ID="ID_1663429215" CREATED="1558028842199" MODIFIED="1558028848477"/>
<node TEXT="EFFECT_DOUBLE_CLICK" ID="ID_58662803" CREATED="1558028852121" MODIFIED="1558028857051"/>
<node TEXT="EFFECT_HEAVY_CLICK" ID="ID_1799524009" CREATED="1558028857477" MODIFIED="1558028863260"/>
<node TEXT="EFFECT_TICK" ID="ID_1290553774" CREATED="1558028863732" MODIFIED="1558028866659"/>
</node>
<node TEXT="Methods" ID="ID_1580394344" CREATED="1558028876354" MODIFIED="1558028878673">
<node TEXT="createOnShot" ID="ID_1371316482" CREATED="1558028879885" MODIFIED="1558028883826"/>
<node TEXT="createPredefined" ID="ID_140700264" CREATED="1558028884522" MODIFIED="1558028889159"/>
<node TEXT="createWaveform" ID="ID_1414973219" CREATED="1558028915213" MODIFIED="1558028920254"/>
</node>
</node>
</node>
<node TEXT="check if device has haptics:" ID="ID_1991496897" CREATED="1558029068124" MODIFIED="1558029077869">
<node TEXT="vibrator.hasVibrator();" ID="ID_1483620230" CREATED="1558029078587" MODIFIED="1558029092360"/>
<node TEXT="vibrator.hasAmplitudeControl()" ID="ID_1230458289" CREATED="1558029100278" MODIFIED="1558029109908"/>
</node>
<node TEXT="depricated (as of API26)" ID="ID_688088413" CREATED="1558028373112" MODIFIED="1558028388413">
<node TEXT="Manifest" ID="ID_698505996" CREATED="1558028390302" MODIFIED="1558028395277">
<node TEXT="&lt;uses-permission android:name=&quot;android.permission.VIBRATE&quot;/&gt;" ID="ID_1788961250" CREATED="1558028406592" MODIFIED="1558028407787"/>
</node>
<node TEXT="Snippet" ID="ID_575729256" CREATED="1558028411947" MODIFIED="1558028415368">
<node TEXT="Vibrator vibrator = (Vibrator) getSystemService(Context.VIBRATOR_SERVICE);&#xa;if (vibrator.hasVibrator())&#xa;{&#xa;  vibrator.vibrate(500); // for 500 ms&#xa;}" ID="ID_56651383" CREATED="1558028422568" MODIFIED="1558028458743"/>
</node>
<node TEXT="Custom Patterns:" ID="ID_544090281" CREATED="1558028499249" MODIFIED="1558028505326">
<node TEXT="private void customVibratePatternNoRepeat()&#xa;{&#xa;    // 0 : Start without a delay&#xa;    // 400 : Vibrate for 400 milliseconds&#xa;    // 200 : Pause for 200 milliseconds&#xa;    // 400 : Vibrate for 400 milliseconds&#xa;    long[] mVibratePattern = new long[]{0, 400, 200, 400};&#xa;&#xa;    // -1 : Do not repeat this pattern&#xa;    // pass 0 if you want to repeat this pattern from 0th index&#xa;    vibrator.vibrate(mVibratePattern, -1);&#xa;}" ID="ID_1385360668" CREATED="1558028652049" MODIFIED="1558028666809"/>
</node>
</node>
<node TEXT="New API" ID="ID_31784039" CREATED="1558028781606" MODIFIED="1558029065088">
<node TEXT="Amplitude" ID="ID_1785608581" CREATED="1558028573794" MODIFIED="1558028576607">
<node TEXT="@RequiresApi(api = Build.VERSION_CODES.O)&#xa;private void createOneShotVibrationUsingVibrationEffect()&#xa;{&#xa;   // 1000 : Vibrate for 1 sec&#xa;   // VibrationEffect.DEFAULT_AMPLITUDE - would perform vibration at full strength&#xa;   VibrationEffect effect = VibrationEffect.createOneShot(1000, VibrationEffect.DEFAULT_AMPLITUDE);&#xa;   vibrator.vibrate(effect);&#xa; }" ID="ID_806439244" CREATED="1558028591634" MODIFIED="1558028609653"/>
</node>
</node>
</node>
</node>
<node TEXT="UWP" FOLDED="true" ID="ID_1553366515" CREATED="1558021210397" MODIFIED="1558034158544">
<node TEXT="sample:" ID="ID_1348676559" CREATED="1558021237173" MODIFIED="1558021853405" LINK="https://github.com/Microsoft/Windows-iotcore-samples/tree/develop/Samples/VibrationDevice"/>
<node TEXT="related StackOverflow" ID="ID_254783720" CREATED="1558027724136" MODIFIED="1558027741150" LINK="https://stackoverflow.com/questions/48989948/how-to-declare-permission-for-haptic-classes-vibrationdevice-in-uwp"/>
<node TEXT="official docs" ID="ID_746193800" CREATED="1558028138436" MODIFIED="1558028148691" LINK="https://docs.microsoft.com/en-us/uwp/api/windows.devices.haptics"/>
<node TEXT="Waveforms" ID="ID_859278014" CREATED="1558022877327" MODIFIED="1558022882278">
<node TEXT="BuzzContinuous" ID="ID_1918485725" CREATED="1558022882704" MODIFIED="1558022895936"/>
<node TEXT="Click" ID="ID_1229472850" CREATED="1558022896519" MODIFIED="1558022898816"/>
<node TEXT="Press" ID="ID_1272991" CREATED="1558022899468" MODIFIED="1558022900818"/>
<node TEXT="Release" ID="ID_1799476339" CREATED="1558022901402" MODIFIED="1558022902821"/>
<node TEXT="RumbleContinuous" ID="ID_1094367383" CREATED="1558022903495" MODIFIED="1558022916164"/>
</node>
<node TEXT="SimpleHapticsControlFeedback" ID="ID_386263456" CREATED="1558022963165" MODIFIED="1558022971830"/>
<node TEXT="Process:" ID="ID_622005877" CREATED="1558023004657" MODIFIED="1558023007110">
<node TEXT="1. Get vibration device" ID="ID_127115400" CREATED="1558023007537" MODIFIED="1558023014491">
<node TEXT="On UI Thread" ID="ID_300139564" CREATED="1558023027157" MODIFIED="1558023031367"/>
<node TEXT="var accessStatus = await VibrationDevice.RequestAccessAsync();" ID="ID_1463807105" CREATED="1558023032512" MODIFIED="1558023048625"/>
<node TEXT="if (accessStatus == VibrationAccessStatus.Allowed)" ID="ID_873920857" CREATED="1558023060121" MODIFIED="1558023079990">
<node ID="ID_1073550505" CREATED="1558023090582" MODIFIED="1558023090582"><richcontent TYPE="NODE">

<html>
  <head>
    
  </head>
  <body>
    <span charset="utf-8" class="pl-smi" style="color: rgb(36, 41, 46); font-family: SFMono-Regular, Consolas, Liberation Mono, Menlo, Courier, monospace; font-size: 12px; font-style: normal; font-weight: 400; letter-spacing: normal; text-align: start; text-indent: 0px; text-transform: none; white-space: pre; word-spacing: 0px; background-color: rgb(255, 255, 255)"><font color="rgb(36, 41, 46)" face="SFMono-Regular, Consolas, Liberation Mono, Menlo, Courier, monospace" size="12px">_vibrationDevice</font></span><font color="rgb(36, 41, 46)" face="SFMono-Regular, Consolas, Liberation Mono, Menlo, Courier, monospace" size="12px"><span style="color: rgb(36, 41, 46); font-family: SFMono-Regular, Consolas, Liberation Mono, Menlo, Courier, monospace; font-size: 12px; font-style: normal; font-weight: 400; letter-spacing: normal; text-align: start; text-indent: 0px; text-transform: none; white-space: pre; word-spacing: 0px; background-color: rgb(255, 255, 255); display: inline !important; float: none"> </span></font><span class="pl-k" style="color: rgb(215, 58, 73); font-family: SFMono-Regular, Consolas, Liberation Mono, Menlo, Courier, monospace; font-size: 12px; font-style: normal; font-weight: 400; letter-spacing: normal; text-align: start; text-indent: 0px; text-transform: none; white-space: pre; word-spacing: 0px; background-color: rgb(255, 255, 255)"><font color="rgb(215, 58, 73)" face="SFMono-Regular, Consolas, Liberation Mono, Menlo, Courier, monospace" size="12px">=</font></span><span style="color: rgb(36, 41, 46); font-family: SFMono-Regular, Consolas, Liberation Mono, Menlo, Courier, monospace; font-size: 12px; font-style: normal; font-weight: 400; letter-spacing: normal; text-align: start; text-indent: 0px; text-transform: none; white-space: pre; word-spacing: 0px; background-color: rgb(255, 255, 255); display: inline !important; float: none"><font color="rgb(36, 41, 46)" face="SFMono-Regular, Consolas, Liberation Mono, Menlo, Courier, monospace" size="12px"> </font></span><span class="pl-k" style="color: rgb(215, 58, 73); font-family: SFMono-Regular, Consolas, Liberation Mono, Menlo, Courier, monospace; font-size: 12px; font-style: normal; font-weight: 400; letter-spacing: normal; text-align: start; text-indent: 0px; text-transform: none; white-space: pre; word-spacing: 0px; background-color: rgb(255, 255, 255)"><font color="rgb(215, 58, 73)" face="SFMono-Regular, Consolas, Liberation Mono, Menlo, Courier, monospace" size="12px">await</font></span><span style="color: rgb(36, 41, 46); font-family: SFMono-Regular, Consolas, Liberation Mono, Menlo, Courier, monospace; font-size: 12px; font-style: normal; font-weight: 400; letter-spacing: normal; text-align: start; text-indent: 0px; text-transform: none; white-space: pre; word-spacing: 0px; background-color: rgb(255, 255, 255); display: inline !important; float: none"><font color="rgb(36, 41, 46)" face="SFMono-Regular, Consolas, Liberation Mono, Menlo, Courier, monospace" size="12px"> </font></span><font color="rgb(36, 41, 46)" face="SFMono-Regular, Consolas, Liberation Mono, Menlo, Courier, monospace" size="12px"><span class="pl-smi" style="color: rgb(36, 41, 46); font-family: SFMono-Regular, Consolas, Liberation Mono, Menlo, Courier, monospace; font-size: 12px; font-style: normal; font-weight: 400; letter-spacing: normal; text-align: start; text-indent: 0px; text-transform: none; white-space: pre; word-spacing: 0px; background-color: rgb(255, 255, 255)">VibrationDevice</span><span style="color: rgb(36, 41, 46); font-family: SFMono-Regular, Consolas, Liberation Mono, Menlo, Courier, monospace; font-size: 12px; font-style: normal; font-weight: 400; letter-spacing: normal; text-align: start; text-indent: 0px; text-transform: none; white-space: pre; word-spacing: 0px; background-color: rgb(255, 255, 255); display: inline !important; float: none">.</span></font><span class="pl-en" style="color: rgb(111, 66, 193); font-family: SFMono-Regular, Consolas, Liberation Mono, Menlo, Courier, monospace; font-size: 12px; font-style: normal; font-weight: 400; letter-spacing: normal; text-align: start; text-indent: 0px; text-transform: none; white-space: pre; word-spacing: 0px; background-color: rgb(255, 255, 255)"><font color="rgb(111, 66, 193)" face="SFMono-Regular, Consolas, Liberation Mono, Menlo, Courier, monospace" size="12px">GetDefaultAsync</font></span><span style="color: rgb(36, 41, 46); font-family: SFMono-Regular, Consolas, Liberation Mono, Menlo, Courier, monospace; font-size: 12px; font-style: normal; font-weight: 400; letter-spacing: normal; text-align: start; text-indent: 0px; text-transform: none; white-space: pre; word-spacing: 0px; background-color: rgb(255, 255, 255); display: inline !important; float: none"><font color="rgb(36, 41, 46)" face="SFMono-Regular, Consolas, Liberation Mono, Menlo, Courier, monospace" size="12px">();</font></span>
  </body>
</html>
</richcontent>
</node>
</node>
</node>
<node TEXT="2. Get handle to desired Feedback" ID="ID_895467949" CREATED="1558023111512" MODIFIED="1558023258598">
<node TEXT="foreach (var feedback in _vibrationDevice.SimpleHapticsController.SupportedFeedback)" ID="ID_1001317337" CREATED="1558023142248" MODIFIED="1558023144139">
<node ID="ID_1046208006" CREATED="1558023153070" MODIFIED="1558023219789"><richcontent TYPE="NODE">

<html>
  <head>
    
  </head>
  <body>
    <p>
      if (feedback.Waveform == KnownSimpleHapticsControllerWaveforms.<b><i>Waveform</i></b>)
    </p>
  </body>
</html>
</richcontent>
<font ITALIC="false"/>
<node TEXT="return feedback" ID="ID_999578780" CREATED="1558023260893" MODIFIED="1558023270232"/>
</node>
</node>
</node>
<node TEXT="3. Make haptic" ID="ID_1524383597" CREATED="1558023349634" MODIFIED="1558023374612">
<node ID="ID_1692035080" CREATED="1558023375127" MODIFIED="1558027684591"><richcontent TYPE="NODE">

<html>
  <head>
    
  </head>
  <body>
    <p>
      _vibrationDevice.SimpleHapticsController.SendHapticFeedback(_<b><i>feedback</i></b>);
    </p>
  </body>
</html>

</richcontent>
</node>
<node ID="ID_1377431185" CREATED="1558023375127" MODIFIED="1558028045017"><richcontent TYPE="NODE">

<html>
  <head>
    
  </head>
  <body>
    <p>
      _vibrationDevice.SimpleHapticsController.SendHapticFeedback(_<b><i>feedback, intensity</i></b>);
    </p>
  </body>
</html>

</richcontent>
</node>
<node ID="ID_1498451640" CREATED="1558023375127" MODIFIED="1558028068800"><richcontent TYPE="NODE">

<html>
  <head>
    
  </head>
  <body>
    <p>
      _vibrationDevice.SimpleHapticsController.SendHapticFeedback(_<b><i>feedback, intensity, duration</i></b>);
    </p>
  </body>
</html>

</richcontent>
</node>
<node ID="ID_1879700760" CREATED="1558023375127" MODIFIED="1558028112924"><richcontent TYPE="NODE">

<html>
  <head>
    
  </head>
  <body>
    <p>
      _vibrationDevice.SimpleHapticsController.SendHapticFeedback(_<b><i>feedback, intensity, playCount, replayPauseInterval</i></b>);
    </p>
  </body>
</html>

</richcontent>
</node>
<node TEXT="_vibrationDevice.SimpleHapticsController.StopFeedback()" ID="ID_1084934504" CREATED="1558028159361" MODIFIED="1558028183302"/>
</node>
</node>
</node>
</node>
<node TEXT="Audio" POSITION="right" ID="ID_1283538189" CREATED="1558034137614" MODIFIED="1558034140428">
<edge COLOR="#ff00ff"/>
<node TEXT="UWP" ID="ID_543432811" CREATED="1558034162971" MODIFIED="1558034166370">
<node TEXT="Sample project" ID="ID_964008200" CREATED="1558034166798" MODIFIED="1558034183916" LINK="https://github.com/jcphlux/XamarinAudioManager/blob/master/AudioManager/AudioManager.WinRT.Shared/WinAudioManager.cs"/>
</node>
<node TEXT="iOS" ID="ID_143393566" CREATED="1558097436104" MODIFIED="1558097438559">
<node TEXT="SystemSounds" ID="ID_324373558" CREATED="1558097441999" MODIFIED="1558097451993" LINK="https://github.com/TUNER88/iOSSystemSoundsLibrary"/>
</node>
<node TEXT="Android" ID="ID_945203157" CREATED="1558097439322" MODIFIED="1558097440988">
<node TEXT="SoundEffectConstants" ID="ID_856225937" CREATED="1558097732074" MODIFIED="1558097741437" LINK="https://developer.android.com/reference/android/view/SoundEffectConstants"/>
</node>
</node>
</node>
</map>
