<map version="freeplane 1.3.0">
<!--To view this file, download free mind mapping software Freeplane from http://freeplane.sourceforge.net -->
<node TEXT="Label Fit Renderers" ID="ID_1723255651" CREATED="1283093380553" MODIFIED="1466444440928"><hook NAME="MapStyle">

<map_styles>
<stylenode LOCALIZED_TEXT="styles.root_node">
<stylenode LOCALIZED_TEXT="styles.predefined" POSITION="right">
<stylenode LOCALIZED_TEXT="default" MAX_WIDTH="600" COLOR="#000000" STYLE="as_parent">
<font NAME="SansSerif" SIZE="10" BOLD="false" ITALIC="false"/>
</stylenode>
<stylenode LOCALIZED_TEXT="defaultstyle.details"/>
<stylenode LOCALIZED_TEXT="defaultstyle.note"/>
<stylenode LOCALIZED_TEXT="defaultstyle.floating">
<edge STYLE="hide_edge"/>
<cloud COLOR="#f0f0f0" SHAPE="ROUND_RECT"/>
</stylenode>
</stylenode>
<stylenode LOCALIZED_TEXT="styles.user-defined" POSITION="right">
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
<stylenode LOCALIZED_TEXT="styles.AutomaticLayout" POSITION="right">
<stylenode LOCALIZED_TEXT="AutomaticLayout.level.root" COLOR="#000000">
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
</stylenode>
</stylenode>
</map_styles>
</hook>
<hook NAME="AutomaticEdgeColor" COUNTER="3"/>
<node TEXT="iOS" POSITION="right" ID="ID_1977642996" CREATED="1466444441502" MODIFIED="1466444442597">
<edge COLOR="#ff0000"/>
<node TEXT="OnElementChanged()" ID="ID_1788273092" CREATED="1466444503107" MODIFIED="1466444565122">
<node TEXT="UpdateText()" ID="ID_1749196970" CREATED="1466444512099" MODIFIED="1466444520388"/>
<node TEXT="UpdateLineBreakMode()" ID="ID_1431643960" CREATED="1466444521101" MODIFIED="1466444527748"/>
<node TEXT="UpdateLines()" ID="ID_1540181066" CREATED="1466447766532" MODIFIED="1466447773556">
<font BOLD="true"/>
</node>
<node TEXT="UpdateFit()" ID="ID_83652017" CREATED="1466447774500" MODIFIED="1466447779432">
<font BOLD="true"/>
<node TEXT="Any change Text, LineBreak, or Lines change needs to go through UpdateFit()." ID="ID_1321962988" CREATED="1466447811550" MODIFIED="1466447832573"/>
</node>
<node TEXT="UpdateAlignment()" ID="ID_825090589" CREATED="1466444529953" MODIFIED="1466444538180"/>
<node TEXT="base.OnElementChanged" ID="ID_178929003" CREATED="1466444544912" MODIFIED="1466444550242"/>
</node>
<node TEXT="OnElementPropertyChanged()" ID="ID_1381683155" CREATED="1466444453908" MODIFIED="1466444573291">
<node TEXT="switch(e.PropertyName)" ID="ID_563414390" CREATED="1466444583057" MODIFIED="1466444592523">
<node TEXT="HorizontalTextAlignment" ID="ID_885156403" CREATED="1466444593144" MODIFIED="1466444602378">
<node TEXT="UpdateAlignment()" ID="ID_393250664" CREATED="1466444655711" MODIFIED="1466444663526"/>
</node>
<node TEXT="VerticalTextAlignment" ID="ID_393764255" CREATED="1466444603010" MODIFIED="1466444613721">
<node TEXT="LayoutSubviews()" ID="ID_1928045524" CREATED="1466444671544" MODIFIED="1466444675039"/>
</node>
<node TEXT="TextColor" ID="ID_141966111" CREATED="1466444616240" MODIFIED="1466444618121">
<node TEXT="UpdateText()" ID="ID_39247316" CREATED="1466444681184" MODIFIED="1466444683599"/>
</node>
<node TEXT="FontProperty" ID="ID_432714237" CREATED="1466444620509" MODIFIED="1466444624697">
<node TEXT="UpdateText()" ID="ID_1912265125" CREATED="1466444689886" MODIFIED="1466444693422"/>
</node>
<node TEXT="TextProperty" ID="ID_365652049" CREATED="1466444627954" MODIFIED="1466444631665">
<node TEXT="UpdateText()" ID="ID_1907477459" CREATED="1466444689886" MODIFIED="1466444693422"/>
</node>
<node TEXT="HtmlTextProperty" ID="ID_563527494" CREATED="1466444634216" MODIFIED="1466444638320">
<node TEXT="UpdateText()" ID="ID_1750558972" CREATED="1466444689886" MODIFIED="1466444693422"/>
</node>
<node TEXT="LineBreakMode" ID="ID_1052177954" CREATED="1466444642463" MODIFIED="1466444645224">
<node TEXT="UpdateLineBreakMode()" ID="ID_1886316164" CREATED="1466444706195" MODIFIED="1466444710110"/>
</node>
</node>
</node>
<node TEXT="UpdateAlignment()" ID="ID_165413263" CREATED="1466444726001" MODIFIED="1466444731701">
<node TEXT="Control.TextAlignment = Element.HorizontalTextAlignment.ToNativeTextAlignment ();" ID="ID_378771589" CREATED="1466444740928" MODIFIED="1466444742386"/>
</node>
<node TEXT="UpdateLineBreakMode()" ID="ID_443680913" CREATED="1466444747999" MODIFIED="1466444752609">
<node TEXT="switch(Element.LineBreakMode)" ID="ID_637104016" CREATED="1466444755793" MODIFIED="1466444769209">
<node TEXT="NoWrap" ID="ID_1847658318" CREATED="1466444805594" MODIFIED="1466444807691">
<node TEXT="Control.LineBreakMode = UILineBreakMode.Clip;" ID="ID_24676955" CREATED="1466444840192" MODIFIED="1466444841630"/>
</node>
<node TEXT="WordWrap" ID="ID_392747259" CREATED="1466444810360" MODIFIED="1466444812556">
<node TEXT="Control.LineBreakMode = UILineBreakMode.WordWrap;" ID="ID_1327435805" CREATED="1466444848017" MODIFIED="1466444848017"/>
</node>
<node TEXT="CharacterWrap" ID="ID_1130060442" CREATED="1466444813201" MODIFIED="1466444817060">
<node TEXT="Control.LineBreakMode = UILineBreakMode.CharacterWrap;" ID="ID_342360904" CREATED="1466444854356" MODIFIED="1466444855594"/>
</node>
<node TEXT="HeadTruncation" ID="ID_640683572" CREATED="1466444818092" MODIFIED="1466444824203">
<node TEXT="Control.LineBreakMode = UILineBreakMode.HeadTruncation;" ID="ID_1648675153" CREATED="1466444861778" MODIFIED="1466444862498"/>
</node>
<node TEXT="TailTruncation" ID="ID_52579205" CREATED="1466444824812" MODIFIED="1466444827083">
<node TEXT="Control.LineBreakMode = UILineBreakMode.TailTruncation;" ID="ID_144199911" CREATED="1466444868163" MODIFIED="1466444868778"/>
</node>
<node TEXT="MiddleTruncation" ID="ID_1617766098" CREATED="1466444827420" MODIFIED="1466444833019">
<node TEXT="Control.LineBreakMode = UILineBreakMode.MiddleTruncation;" ID="ID_98705154" CREATED="1466444881462" MODIFIED="1466444881462"/>
</node>
</node>
</node>
<node TEXT="UpdateText()" ID="ID_914988204" CREATED="1466444891214" MODIFIED="1466444895073">
<node TEXT="perfectSizeValidated = false;" ID="ID_526106049" CREATED="1466445322107" MODIFIED="1466445327655"/>
<node TEXT="Control.Font = Element.ToUIFont();" ID="ID_1393446538" CREATED="1466445330595" MODIFIED="1466445342454"/>
<node TEXT="var color = (Color)Element.GetValue(Label.TextColorProperty);" ID="ID_333128418" CREATED="1466445356669" MODIFIED="1466445358287"/>
<node TEXT="Control.TextColor = color.ToUIColor(UIColor.Black);" ID="ID_1332669589" CREATED="1466445363989" MODIFIED="1466445364790"/>
<node TEXT="if (Element.FormattedText == null)" ID="ID_1121916511" CREATED="1466445393628" MODIFIED="1466445404940">
<node TEXT="Control.Text = (string)Element.GetValue(Label.TextProperty);" ID="ID_1148221970" CREATED="1466445410181" MODIFIED="1466445412228"/>
</node>
<node TEXT="else" ID="ID_687464576" CREATED="1466445425579" MODIFIED="1466445426708">
<node TEXT="Control.AttributedText = Element.ToNSAttributedString(Control);" ID="ID_1768645783" CREATED="1466445432773" MODIFIED="1466445433403"/>
</node>
</node>
<node TEXT="LayoutSubviews" ID="ID_1856963565" CREATED="1466444448988" MODIFIED="1466444453116"/>
</node>
<node TEXT="Desired behavior" POSITION="right" ID="ID_135742475" CREATED="1466446239558" MODIFIED="1466446243473">
<edge COLOR="#00ff00"/>
<node TEXT="determine how big the text would be given the current constraints" ID="ID_1580107462" CREATED="1466446244122" MODIFIED="1466446295158">
<node TEXT="Bounds.Height" ID="ID_1302164976" CREATED="1466446295687" MODIFIED="1466446310134"/>
<node TEXT="Bounds.Width" ID="ID_1922331632" CREATED="1466446310652" MODIFIED="1466446313230"/>
<node TEXT="lineHeight =  labelText.sizeWithAttributes([NSFontAttributeName:font.fontWithSize(fontSizeAverage)]).height" ID="ID_1594993113" CREATED="1466446433105" MODIFIED="1466446999196"/>
<node TEXT="CGSize theSize = [text sizeWithFont:[UIFont systemFontOfSize:17.0f] constrainedToSize:CGSizeMake(310.0f, FLT_MAX) lineBreakMode:UILineBreakModeWordWrap];" ID="ID_447132039" CREATED="1466446967283" MODIFIED="1466446969050"/>
</node>
<node TEXT="if (Element.Fit != LabelFit.None)" ID="ID_407176904" CREATED="1466447225592" MODIFIED="1466447280598">
<node TEXT="tmpFontSize = Element.FontSize;" ID="ID_593933978" CREATED="1466447283857" MODIFIED="1466447371416"/>
<node TEXT="minFontSize = Element.MinFontSize;" ID="ID_347523241" CREATED="1466447427432" MODIFIED="1466447442846"/>
<node TEXT="if (minFontSize &lt; 1)" ID="ID_1043251967" CREATED="1466447453150" MODIFIED="1466447485248">
<node TEXT="minFontSize == 4;" ID="ID_432137812" CREATED="1466447485697" MODIFIED="1466447493440"/>
</node>
</node>
</node>
</node>
</map>
