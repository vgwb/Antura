---
title: Paryż! (fr_01) - Script
hide:
---

# Paryż! (fr_01) - Script
> [!note] Educators & Designers: help improving this quest!
> **Comments and feedback**: [discuss in the Forum](https://antura.discourse.group/t/fr-01-paris/23/1)  
> **Improve translations**: [comment the Google Sheet](https://docs.google.com/spreadsheets/d/1FPFOy8CHor5ArSg57xMuPAG7WM27-ecDOiU-OmtHgjw/edit?gid=755037318#gid=755037318)  
> **Improve the script**: [propose an edit here](https://github.com/vgwb/Antura/blob/main/Assets/_discover/_quests/FR_01%20Paris/FR_01%20Paris%20-%20Yarn%20Script.yarn)  

<a id="ys-node-quest-start"></a>

## quest_start

<div class="yarn-node" data-title="quest_start">
<pre class="yarn-code" style="--node-color:red"><code>
<span class="yarn-header-dim">// fr_01 | Paris</span>
<span class="yarn-header-dim">// </span>
<span class="yarn-header-dim">tags:</span>
<span class="yarn-header-dim">type: panel</span>
<span class="yarn-header-dim">color: red</span>
<span class="yarn-header-dim">actor: NARRATOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;set $TOTAL_COINS = 0&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;set $COLLECTED_ITEMS = 0&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $QUEST_ITEMS = 0&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $MET_GUIDE = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $MET_MAJOR = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $MET_MONALISA = false&gt;&gt;</span>

<span class="yarn-line">Witamy w Paryżu!</span> <span class="yarn-meta">#line:start </span>
<span class="yarn-line">Idź porozmawiać z korepetytorem!</span> <span class="yarn-meta">#line:start_2</span>
<span class="yarn-cmd">&lt;&lt;target tutor&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-quest-end"></a>

## quest_end

<div class="yarn-node" data-title="quest_end">
<pre class="yarn-code" style="--node-color:green"><code>
<span class="yarn-header-dim">color: green</span>
<span class="yarn-header-dim">panel: panel_endgame</span>
<span class="yarn-header-dim">actor: NARRATOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">ŚWIETNIE! Teraz mogę upiec bagietkę. I...</span> <span class="yarn-meta">#line:0017917 </span>
<span class="yarn-line">GRATULACJE! Wygrałeś grę! Podobała Ci się?</span> <span class="yarn-meta">#line:0d11596 </span>
<span class="yarn-cmd">&lt;&lt;jump post_quest_activity&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-post-quest-activity"></a>

## post_quest_activity

<div class="yarn-node" data-title="post_quest_activity">
<pre class="yarn-code" style="--node-color:green"><code>
<span class="yarn-header-dim">color: green</span>
<span class="yarn-header-dim">panel: panel</span>
<span class="yarn-header-dim">tags: proposal</span>
<span class="yarn-header-dim">actor: NARRATOR</span>

<span class="yarn-header-dim">---</span>
<span class="yarn-line">Dlaczego nie narysujesz Wieży Eiffla?</span> <span class="yarn-meta">#line:002620f </span>
<span class="yarn-cmd">&lt;&lt;quest_end&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-talk-tutor"></a>

## talk_tutor

<div class="yarn-node" data-title="talk_tutor">
<pre class="yarn-code" style="--node-color:blue"><code>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Widziałem, jak Antura udał się na Wieżę Eiffla.</span> <span class="yarn-meta">#line:talk_tutor</span>
<span class="yarn-cmd">&lt;&lt;camera_focus tour_eiffell&gt;&gt;</span>
<span class="yarn-line">Podążaj za światłem lub skorzystaj z mapy!</span> <span class="yarn-meta">#line:talk_tutor_2 </span>
<span class="yarn-line">Idź tam teraz!</span> <span class="yarn-meta">#line:talk_tutor_3 </span>

</code>
</pre>
</div>

<a id="ys-node-talk-eiffell-roof"></a>

## talk_eiffell_roof

<div class="yarn-node" data-title="talk_eiffell_roof">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: toureiffel</span>
<span class="yarn-header-dim">actor: GUIDE_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;asset toureiffell&gt;&gt;</span>
<span class="yarn-line">Wieża Eiffla ma 300 metrów wysokości.</span> <span class="yarn-meta">#line:08c1973 </span>
<span class="yarn-cmd">&lt;&lt;asset mr_eiffel&gt;&gt;</span>
<span class="yarn-line">Zbudowany przez pana Eiffela w 1887 roku.</span> <span class="yarn-meta">#line:09e5c3b </span>
<span class="yarn-cmd">&lt;&lt;asset iron&gt;&gt;</span>
<span class="yarn-line">Jest z żelaza!</span> <span class="yarn-meta">#line:0d59ade </span>
<span class="yarn-line">Widziałem Anturę zmierzającą w kierunku Notre Dame.</span> <span class="yarn-meta">#line:04d1e52 </span>
<span class="yarn-cmd">&lt;&lt;camera_focus notredame&gt;&gt;</span>
<span class="yarn-line">Jedź tam!</span> <span class="yarn-meta">#line:083b3bf </span>


</code>
</pre>
</div>

<a id="ys-node-talk-eiffell-guide"></a>

## talk_eiffell_guide

<div class="yarn-node" data-title="talk_eiffell_guide">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: toureiffel</span>
<span class="yarn-header-dim">actor: GUIDE_F</span>
<span class="yarn-header-dim">---</span>
&lt;&lt;if $TOTAL_COINS &gt; 2&gt;&gt;
<span class="yarn-line">    Oto twój bilet.</span> <span class="yarn-meta">#line:04e74ad </span>
    <span class="yarn-cmd">&lt;&lt;asset tour_eiffell_ticket&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $TOTAL_COINS = $TOTAL_COINS-3&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;action area_toureiffel&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;asset tour_eiffell_map&gt;&gt;</span>
<span class="yarn-line">    Widziałem Anturę wchodzącego na szczyt wieży.</span> <span class="yarn-meta">#line:089abda </span>
<span class="yarn-line">    Jedź windą!</span> <span class="yarn-meta">#line:0585a5e </span>
&lt;&lt;elseif $TOTAL_COINS &gt; 0&gt;&gt; 
<span class="yarn-line">    Zbierz wszystkie monety!</span> <span class="yarn-meta">#line:04d966b </span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Bilet na Wieżę Eiffla kosztuje 3 monety.</span> <span class="yarn-meta">#line:069cbb3 </span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-talk-notre-dame"></a>

## talk_notre_dame

<div class="yarn-node" data-title="talk_notre_dame">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: notredame</span>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Jestem merem Paryża.</span> <span class="yarn-meta">#line:0cc11fa </span>
<span class="yarn-line">To jest katedra Notre-Dame.</span> <span class="yarn-meta">#line:06f3fa2 </span>
<span class="yarn-cmd">&lt;&lt;card notredame zoom&gt;&gt;</span>
<span class="yarn-line">Jest to słynny kościół gotycki, zbudowany w 1182 roku.</span> <span class="yarn-meta">#line:02edc0f </span>
<span class="yarn-cmd">&lt;&lt;action AREA_NOTREDAME_ROOF&gt;&gt;</span>
<span class="yarn-line">Chodź ze mną na dach kościoła!</span> <span class="yarn-meta">#line:083dfcc </span>
<span class="yarn-cmd">&lt;&lt;set $MET_MAJOR = true&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-talk-notre-dame-roof"></a>

## talk_notre_dame_roof

<div class="yarn-node" data-title="talk_notre_dame_roof">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: notredame</span>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;asset notredame_fire&gt;&gt;</span>
<span class="yarn-line">W 2019 roku wybuchł duży pożar, ale udało się go naprawić.</span> <span class="yarn-meta">#line:09a0ead </span>
<span class="yarn-line">Widziałem, jak Antura wbiegł do Luwru.</span> <span class="yarn-meta">#line:02ba888 </span>
<span class="yarn-line">Leży tuż za rzeką Sekwaną.</span> <span class="yarn-meta">#line:00d22e5 </span>

</code>
</pre>
</div>

<a id="ys-node-gargoyle"></a>

## gargoyle

<div class="yarn-node" data-title="gargoyle">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: notredame</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Spójrz na tę statuę!</span> <span class="yarn-meta">#line:0f7f9d8 </span>
<span class="yarn-cmd">&lt;&lt;card gargoyle zoom&gt;&gt;</span>
<span class="yarn-line">Czy to nie jest straszne?</span> <span class="yarn-meta">#line:0b5d057 </span>

</code>
</pre>
</div>

<a id="ys-node-talk-louvre-external"></a>

## talk_louvre_external

<div class="yarn-node" data-title="talk_louvre_external">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: louvre</span>
<span class="yarn-header-dim">actor: SENIOR_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $MET_MONALISA&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card go_bakery&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;asset louvre&gt;&gt;</span>
<span class="yarn-line">kobieta: To jest wejście do Luwru, naszego narodowego muzeum sztuki.</span> <span class="yarn-meta">#line:0cf1cc8 </span>
<span class="yarn-line">kobieta: Chcesz wejść?</span> <span class="yarn-meta">#line:0f74ff9</span>
<span class="yarn-line">Tak</span> <span class="yarn-meta">#line:090114f </span>
<span class="yarn-line">    Życzymy miłej wizyty!</span> <span class="yarn-meta">#line:056e051 </span>
    <span class="yarn-cmd">&lt;&lt;action AREA_LOUVRE_ENTER &gt;&gt;</span>
<span class="yarn-line">NIE</span> <span class="yarn-meta">#line:077422a </span>
<span class="yarn-line">    Dobra.</span> <span class="yarn-meta">#line:0c28ea0 #do_not_translate</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-talk-louvre-guide"></a>

## talk_louvre_guide

<div class="yarn-node" data-title="talk_louvre_guide">
<pre class="yarn-code" style="--node-color:blue"><code>
<span class="yarn-header-dim">group: louvre</span>
<span class="yarn-header-dim">actor: ADULT_F</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Witamy w Luwrze. Co chcesz zrobić?</span> <span class="yarn-meta">#line:0e6d2a5 </span>
<span class="yarn-line">Opowiedz mi o Luwrze</span> <span class="yarn-meta">#line:0a5fc63 </span>
    <span class="yarn-cmd">&lt;&lt;jump visit_louvre&gt;&gt;</span>
<span class="yarn-line">Wyjście</span> <span class="yarn-meta">#line:0efc18f </span>
    <span class="yarn-cmd">&lt;&lt;if $MET_MONALISA&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;action AREA_LOUVRE_EXIT&gt;&gt;</span>
<span class="yarn-line">        Wracać!</span> <span class="yarn-meta">#line:07dd921 </span>
    <span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;jump find_monalisa&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-louvre-monalisa"></a>

## louvre_monalisa

<div class="yarn-node" data-title="louvre_monalisa">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">group: louvre</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">actor: ADULT_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;asset monalisa&gt;&gt;</span>
<span class="yarn-line">To jest słynna Mona Lisa.</span> <span class="yarn-meta">#line:louvre_monalisa_1</span>
<span class="yarn-cmd">&lt;&lt;set $MET_MONALISA = true&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;asset leaonardodavinci&gt;&gt;</span>
<span class="yarn-line">Leonardo namalował ten obraz około 1500 roku.</span> <span class="yarn-meta">#line:louvre_monalisa_2</span>
<span class="yarn-line">przez artystę Leonarda da Vinci.</span> <span class="yarn-meta">#line:louvre_monalisa_3</span>

</code>
</pre>
</div>

<a id="ys-node-louvre-liberty"></a>

## louvre_liberty

<div class="yarn-node" data-title="louvre_liberty">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">group: louvre</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">actor: ADULT_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;asset liberty_leading&gt;&gt;</span>
<span class="yarn-line">Ten obraz przedstawia wolność.</span> <span class="yarn-meta">#line:louvre_liberty_1</span>
<span class="yarn-line">Nazywa się Wolność wiodąca lud na barykady.</span> <span class="yarn-meta">#line:louvre_liberty_2</span>
<span class="yarn-line">autorstwa francuskiego artysty Eugène'a Delacroix.</span> <span class="yarn-meta">#line:louvre_liberty_3</span>

</code>
</pre>
</div>

<a id="ys-node-louvre-venus"></a>

## louvre_venus

<div class="yarn-node" data-title="louvre_venus">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">group: louvre</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">actor: ADULT_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;asset venusmilo&gt;&gt;</span>
<span class="yarn-line">Wenus z Milo, starożytna grecka rzeźba marmurowa.</span> <span class="yarn-meta">#line:053d4fe </span>

</code>
</pre>
</div>

<a id="ys-node-npc-louvre-pyramid"></a>

## npc_louvre_pyramid

<div class="yarn-node" data-title="npc_louvre_pyramid">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">actor: GUIDE_F</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">spawn_group: louvre</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card louvre_pyramid&gt;&gt;</span>
<span class="yarn-line">Ta szklana piramida stanowi główne wejście do muzeum.</span> <span class="yarn-meta">#line:fr01_pyramid_1</span>
<span class="yarn-line">Zbudowano go w latach 80. XX wieku, aby móc przyjąć większą liczbę turystów.</span> <span class="yarn-meta">#line:fr01_pyramid_2</span>

</code>
</pre>
</div>

<a id="ys-node-npc-code-of-hammurabi"></a>

## npc_code_of_hammurabi

<div class="yarn-node" data-title="npc_code_of_hammurabi">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">actor: GUIDE_F</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">spawn_group: louvre</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card code_of_hammurabi&gt;&gt;</span>
<span class="yarn-line">Na tym kamieniu znajdują się bardzo stare prawa ze starożytnej Mezopotamii.</span> <span class="yarn-meta">#line:fr01_hammurabi_1</span>
<span class="yarn-line">Zostały napisane prawie 4000 lat temu.</span> <span class="yarn-meta">#line:fr01_hammurabi_2</span>

</code>
</pre>
</div>

<a id="ys-node-npc-coronation-of-napoleon-david"></a>

## npc_coronation_of_napoleon_david

<div class="yarn-node" data-title="npc_coronation_of_napoleon_david">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">actor: GUIDE_M</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">spawn_group: louvre</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card coronation_of_napoleon_david&gt;&gt;</span>
<span class="yarn-line">Na tym dużym obrazie widać Napoleona obejmującego władzę jako cesarz.</span> <span class="yarn-meta">#line:fr01_coronation_1</span>
<span class="yarn-line">Artysta Jacques-Louis David namalował wiele szczegółów.</span> <span class="yarn-meta">#line:fr01_coronation_2</span>

</code>
</pre>
</div>

<a id="ys-node-npc-oath-of-the-horatii-david"></a>

## npc_oath_of_the_horatii_david

<div class="yarn-node" data-title="npc_oath_of_the_horatii_david">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">actor: GUIDE_M</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">spawn_group: louvre</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card oath_of_the_horatii_david&gt;&gt;</span>
<span class="yarn-line">Na tym obrazie widać braci składających odważną obietnicę.</span> <span class="yarn-meta">#line:fr01_horatii_1</span>
<span class="yarn-line">Uczy obowiązku i odwagi zaczerpniętych ze starożytnego Rzymu.</span> <span class="yarn-meta">#line:fr01_horatii_2</span>

</code>
</pre>
</div>

<a id="ys-node-npc-the-seated-scribe"></a>

## npc_the_seated_scribe

<div class="yarn-node" data-title="npc_the_seated_scribe">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">actor: GUIDE_F</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">spawn_group: louvre</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card the_seated_scribe&gt;&gt;</span>
<span class="yarn-line">Ta statua przedstawia mężczyznę piszącego w starożytnym Egipcie.</span> <span class="yarn-meta">#line:fr01_scribe_1</span>
<span class="yarn-line">Jego oczy wyglądają bardzo realistycznie i jasno.</span> <span class="yarn-meta">#line:fr01_scribe_2</span>

</code>
</pre>
</div>

<a id="ys-node-npc-winged-victory-of-samothrace"></a>

## npc_winged_victory_of_samothrace

<div class="yarn-node" data-title="npc_winged_victory_of_samothrace">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">actor: GUIDE_M</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">spawn_group: louvre</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card winged_victory_of_samothrace&gt;&gt;</span>
<span class="yarn-line">Ta statua przedstawia uskrzydloną postać lądującą na statku.</span> <span class="yarn-meta">#line:fr01_victory_1</span>
<span class="yarn-line">Wiatr kształtuje jego ubrania i skrzydła.</span> <span class="yarn-meta">#line:fr01_victory_2</span>

</code>
</pre>
</div>

<a id="ys-node-talk-cook"></a>

## talk_cook

<div class="yarn-node" data-title="talk_cook">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: bakery</span>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">---</span>
&lt;&lt;if $COLLECTED_ITEMS &gt;= 4&gt;&gt;
    <span class="yarn-cmd">&lt;&lt;jump quest_end&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Ratunku! Antura narobiła bałaganu w mojej kuchni!</span> <span class="yarn-meta">#line:07bbb10 </span>
<span class="yarn-line">    Nie mogę znaleźć składników potrzebnych do zrobienia bagietki.</span> <span class="yarn-meta">#line:09e867c </span>
    <span class="yarn-cmd">&lt;&lt;asset  baguette&gt;&gt;</span>
<span class="yarn-line">    Nasz specjalny chleb francuski!</span> <span class="yarn-meta">#line:0874503 </span>
    <span class="yarn-cmd">&lt;&lt;set $QUEST_ITEMS = 4&gt;&gt;</span>
<span class="yarn-line">    Proszę przynieść mi 4 składniki:</span> <span class="yarn-meta">#line:07d64c7 </span>
<span class="yarn-line">    mąka, woda, drożdże i sól.</span> <span class="yarn-meta">#line:0c01530 </span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>


</code>
</pre>
</div>

<a id="ys-node-visit-louvre"></a>

## visit_louvre

<div class="yarn-node" data-title="visit_louvre">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: louvre</span>
<span class="yarn-header-dim">actor: ADULT_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;asset louvre_inside&gt;&gt;</span>
<span class="yarn-line">Można tu znaleźć wiele rzeźb i obrazów.</span> <span class="yarn-meta">#line:08dc97f </span>
<span class="yarn-cmd">&lt;&lt;jump find_monalisa&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-find-monalisa"></a>

## find_monalisa

<div class="yarn-node" data-title="find_monalisa">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: louvre</span>
<span class="yarn-header-dim">actor: ADULT_F</span>
<span class="yarn-header-dim">---</span>

<span class="yarn-cmd">&lt;&lt;action monalisa&gt;&gt;</span>
<span class="yarn-line">Znajdź Monę Lisę!</span> <span class="yarn-meta">#line:0442392 </span>

</code>
</pre>
</div>

<a id="ys-node-go-bakery"></a>

## go_bakery

<div class="yarn-node" data-title="go_bakery">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: louvre</span>
<span class="yarn-header-dim">actor: SENIOR_F</span>
<span class="yarn-header-dim">---</span>
 
<span class="yarn-line">A teraz szukajcie Antury! Poszła do piekarni po bagietkę!</span> <span class="yarn-meta">#line:076ef0f </span>
<span class="yarn-line">Zwijać się!</span> <span class="yarn-meta">#line:0e9c3e7 </span>

</code>
</pre>
</div>

<a id="ys-node-baguette-salt"></a>

## baguette_salt

<div class="yarn-node" data-title="baguette_salt">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">group: bakery</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;action COLLECT_1&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;card baguette_salt&gt;&gt;</span>
<span class="yarn-line">To jest sól.</span> <span class="yarn-meta">#line:00f1d2f </span>

</code>
</pre>
</div>

<a id="ys-node-baguette-flour"></a>

## baguette_flour

<div class="yarn-node" data-title="baguette_flour">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">group: bakery</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;action COLLECT_2&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;card baguette_flour&gt;&gt;</span>
<span class="yarn-line">To jest mąka.</span> <span class="yarn-meta">#line:06022b0 </span>

</code>
</pre>
</div>

<a id="ys-node-baguette-water"></a>

## baguette_water

<div class="yarn-node" data-title="baguette_water">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">group: bakery</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;action COLLECT_3&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;card baguette_water&gt;&gt;</span>
<span class="yarn-line">To jest woda.</span> <span class="yarn-meta">#line:0c4d1f6 </span>

</code>
</pre>
</div>

<a id="ys-node-baguette-yeast"></a>

## baguette_yeast

<div class="yarn-node" data-title="baguette_yeast">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">group: bakery</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;action COLLECT_4&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;card baguette_yeast&gt;&gt;</span>
<span class="yarn-line">To są drożdże.</span> <span class="yarn-meta">#line:025865d </span>

</code>
</pre>
</div>

<a id="ys-node-npc-french-guide"></a>

## npc_french_guide

<div class="yarn-node" data-title="npc_french_guide">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">///////// NPCs SPAWNED IN THE SCENE //////////</span>
<span class="yarn-header-dim">// these npc are spawn automatically in the scene</span>
<span class="yarn-header-dim">// use these to add random facts. everythime you meet them</span>
<span class="yarn-header-dim">// they will say one of these lines randomly</span>
<span class="yarn-header-dim">actor: ADULT_F</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">spawn_group: generic</span>

<span class="yarn-header-dim">---</span>
<span class="yarn-line">Cześć. Co chcesz wiedzieć?</span> <span class="yarn-meta">#line:0070084 </span>
<span class="yarn-line">Czym jest Wieża Eiffla?</span> <span class="yarn-meta">#line:0d91dc0 </span>
<span class="yarn-line">    Słynna żelazna wieża o wysokości 300 metrów.</span> <span class="yarn-meta">#line:0f17af0 </span>
<span class="yarn-line">    Symbol Paryża!</span> <span class="yarn-meta">#line:07a113f </span>
<span class="yarn-line">Gdzie jesteśmy?</span> <span class="yarn-meta">#line:09dd1da </span>
<span class="yarn-line">    Jesteśmy w Paryżu, mieście miłości!</span> <span class="yarn-meta">#line:02b627d </span>
<span class="yarn-line">Czy to miejsce jest prawdziwe?</span> <span class="yarn-meta">#line:08bede4 </span>
<span class="yarn-line">    Jasne! Dlaczego pytasz?</span> <span class="yarn-meta">#line:08654e6 </span>
<span class="yarn-line">    No cóż... wygląda jak gra wideo, prawda?</span> <span class="yarn-meta">#line:0bc62a3 </span>
<span class="yarn-line">Nic. Pa.</span> <span class="yarn-meta">#line:0fe0732 </span>

</code>
</pre>
</div>

<a id="ys-node-spawned-eiffell-tourist"></a>

## spawned_eiffell_tourist

<div class="yarn-node" data-title="spawned_eiffell_tourist">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">actor: ADULT_M</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">spawn_group: eiffel_tower</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Chciałbym wejść na Wieżę Eiffla.</span> <span class="yarn-meta">#line:0aee9bb </span>
<span class="yarn-line">Aby wejść na górę potrzebny jest bilet.</span> <span class="yarn-meta">#line:09be864 </span>
<span class="yarn-line">W 1889 roku w Paryżu odbył się wielki jarmark.</span> <span class="yarn-meta">#line:0a3f4e1 </span>
<span class="yarn-line">    Miało to upamiętnić setną rocznicę rewolucji francuskiej.</span> <span class="yarn-meta">#line:01fa210 </span>
<span class="yarn-line">    Na to wydarzenie wybudowano Wieżę Eiffla.</span> <span class="yarn-meta">#line:0d6f3c4 </span>
<span class="yarn-line">Uwielbiam Paryż!</span> <span class="yarn-meta">#line:0bda18a </span>

</code>
</pre>
</div>

<a id="ys-node-spawned-man"></a>

## spawned_man

<div class="yarn-node" data-title="spawned_man">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">actor: ADULT_M</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">spawn_group: generic</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Masz jakieś pytania?</span> <span class="yarn-meta">#line:07b94e9 </span>
<span class="yarn-line">Widziałeś Anturę?</span> <span class="yarn-meta">#line:0f18ad3 </span>
<span class="yarn-line">    Tak! Rozmawiaj ze wszystkimi i podążaj za światłami!</span> <span class="yarn-meta">#line:0cf9b4e </span>
<span class="yarn-line">    Nie. Kim jest Antura?</span> <span class="yarn-meta">#line:0f9dd62 </span>
<span class="yarn-line">Co robisz?</span> <span class="yarn-meta">#line:002796f </span>
<span class="yarn-line">    Idę do pracy!</span> <span class="yarn-meta">#line:0fe4ff4 </span>
<span class="yarn-line">    Zamierzam kupić chleb w piekarni.</span> <span class="yarn-meta">#line:05a38a8 </span>
<span class="yarn-line">Skąd pochodzisz?</span> <span class="yarn-meta">#line:05eabcf </span>
<span class="yarn-line">    Nie urodziłem się w tym kraju.</span> <span class="yarn-meta">#line:0635a6a </span>
<span class="yarn-line">    Z planety Ziemia.</span> <span class="yarn-meta">#line:0749690 </span>
<span class="yarn-line">Do widzenia</span> <span class="yarn-meta">#line:0ee51fc </span>

</code>
</pre>
</div>

<a id="ys-node-spawned-kid-m"></a>

## spawned_kid_m

<div class="yarn-node" data-title="spawned_kid_m">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">actor: KID_M</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">spawn_group: kids</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Cześć!</span> <span class="yarn-meta">#line:0c4d9e4 </span>
<span class="yarn-line">Jak się masz?</span> <span class="yarn-meta">#line:032d401 </span>

</code>
</pre>
</div>

<a id="ys-node-spawned-kid-f"></a>

## spawned_kid_f

<div class="yarn-node" data-title="spawned_kid_f">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">actor: KID_F</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">spawn_group: kids</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Dzień dobry!</span> <span class="yarn-meta">#line:041403d </span>
<span class="yarn-line">Ca va?</span> <span class="yarn-meta">#line:04986a3 </span>

</code>
</pre>
</div>

<a id="ys-node-npc-louvre-museum"></a>

## npc_louvre_museum

<div class="yarn-node" data-title="npc_louvre_museum">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">actor: GUIDE_F</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">spawn_group: louvre</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Luwr jest jednym z największych muzeów na świecie.</span> <span class="yarn-meta">#line:fr01_louvre_rand_1</span>
<span class="yarn-line">Można tu spacerować godzinami i nadal nie zobaczyć wszystkiego.</span> <span class="yarn-meta">#line:fr01_louvre_rand_2</span>
<span class="yarn-line">Wiele dzieł sztuki jest tu starszych niż dziadkowie Twoich dziadków.</span> <span class="yarn-meta">#line:fr01_louvre_rand_3</span>
<span class="yarn-line">Szklana piramida przepuszcza światło do znajdujących się poniżej pomieszczeń.</span> <span class="yarn-meta">#line:fr01_louvre_rand_4</span>

</code>
</pre>
</div>

<a id="ys-node-npc-notredame-base"></a>

## npc_notredame_base

<div class="yarn-node" data-title="npc_notredame_base">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">actor: GUIDE_M</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">spawn_group: notredame</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Notre-Dame to słynna gotycka katedra.</span> <span class="yarn-meta">#line:fr01_notredame_base_1</span>
<span class="yarn-line">Budowniczowie rozpoczęli jej budowę ponad 800 lat temu.</span> <span class="yarn-meta">#line:fr01_notredame_base_2</span>
<span class="yarn-line">Wielkie dzwony rozbrzmiewają w całym mieście.</span> <span class="yarn-meta">#line:fr01_notredame_base_3</span>

</code>
</pre>
</div>

<a id="ys-node-npc-notredame-roof"></a>

## npc_notredame_roof

<div class="yarn-node" data-title="npc_notredame_roof">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">actor: GUIDE_M</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">spawn_group: notredame_roof</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Z dachu można zobaczyć większą część Paryża.</span> <span class="yarn-meta">#line:fr01_notredame_roof_1</span>
<span class="yarn-line">Siedzą tu kamienne stworzenia zwane gargulcami.</span> <span class="yarn-meta">#line:fr01_notredame_roof_2</span>
<span class="yarn-line">Robotnicy nadal odnawiają części katedry.</span> <span class="yarn-meta">#line:fr01_notredame_roof_3</span>

</code>
</pre>
</div>

<a id="ys-node-npc-bakery"></a>

## npc_bakery

<div class="yarn-node" data-title="npc_bakery">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">spawn_group: bakery</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Zapach świeżego chleba uszczęśliwia ludzi.</span> <span class="yarn-meta">#line:fr01_bakery_1</span>
<span class="yarn-line">Do przygotowania bagietki używamy mąki, wody, drożdży i soli.</span> <span class="yarn-meta">#line:fr01_bakery_2</span>
<span class="yarn-line">Piekarze wstają bardzo wcześnie rano, aby zacząć wyrabiać ciasto.</span> <span class="yarn-meta">#line:fr01_bakery_3</span>

</code>
</pre>
</div>


