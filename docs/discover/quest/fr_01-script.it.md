---
title: Parigi! (fr_01) - Script
hide:
---

# Parigi! (fr_01) - Script
[Quest Index](./index.it.md) - Language: [english](./fr_01-script.md) - [french](./fr_01-script.fr.md) - [polish](./fr_01-script.pl.md) - italian

!!! note "Educators & Designers: help improving this quest!"
    **Comments and feedback**: [discuss in the Forum](https://vgwb.discourse.group/t/fr-01-paris/23/1)  
    **Improve translations**: [comment the Google Sheet](https://docs.google.com/spreadsheets/d/1FPFOy8CHor5ArSg57xMuPAG7WM27-ecDOiU-OmtHgjw/edit?gid=755037318#gid=755037318)  
    **Improve the script**: [propose an edit here](https://github.com/vgwb/Antura/blob/main/Assets/_discover/_quests/FR_01%20Paris/FR_01%20Paris%20-%20Yarn%20Script.yarn)  

<a id="ys-node-init"></a>
## init

<div class="yarn-node" data-title="init"><pre class="yarn-code" style="--node-color:red"><code><span class="yarn-header-dim">// Quest: fr_01 | Paris</span>
<span class="yarn-header-dim">// </span>
<span class="yarn-header-dim">tags:</span>
<span class="yarn-header-dim">type: panel</span>
<span class="yarn-header-dim">color: red</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;set $TOTAL_COINS = 0&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;set $COLLECTED_ITEMS = 0&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $QUEST_ITEMS = 0&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $MET_GUIDE = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $MET_MAJOR = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $MET_MONALISA = false&gt;&gt;</span>
[MISSING TRANSLATION: ]
<span class="yarn-line">Benvenuti a Parigi! <span class="yarn-meta">#line:fr01_start </span></span>
<span class="yarn-line">[MISSING TRANSLATION: Go and talk with the tutor!] <span class="yarn-meta">#line:fr01_start_2</span></span>
<span class="yarn-cmd">&lt;&lt;target tutor&gt;&gt;</span>
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-the-end"></a>
## the_end

<div class="yarn-node" data-title="the_end"><pre class="yarn-code" style="--node-color:green"><code><span class="yarn-header-dim">color: green</span>
<span class="yarn-header-dim">panel: panel_endgame</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">OTTIMO! Ora posso cuocere la baguette. E... <span class="yarn-meta">#line:0017917 </span></span>
<span class="yarn-line">CONGRATULAZIONI! Hai vinto la partita! Ti è piaciuta? <span class="yarn-meta">#line:0d11596 </span></span>
<span class="yarn-cmd">&lt;&lt;jump quest_proposal&gt;&gt;</span>
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-quest-proposal"></a>
## quest_proposal

<div class="yarn-node" data-title="quest_proposal"><pre class="yarn-code" style="--node-color:green"><code><span class="yarn-header-dim">color: green</span>
<span class="yarn-header-dim">panel: panel</span>
<span class="yarn-header-dim">tags: proposal</span>
<span class="yarn-header-dim">---</span>
[MISSING TRANSLATION: Why don't you draw the Eiffel Tower?]
<span class="yarn-cmd">&lt;&lt;quest_end&gt;&gt;</span>
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-talk-tutor"></a>
## talk_tutor

<div class="yarn-node" data-title="talk_tutor"><pre class="yarn-code" style="--node-color:blue"><code><span class="yarn-header-dim">actor: tutor</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Ho visto Antura andare alla Torre Eiffel. <span class="yarn-meta">#line:fr01_talk_tutor</span></span>
<span class="yarn-cmd">&lt;&lt;camera_focus tour_eiffell&gt;&gt;</span>
<span class="yarn-line">Segui il bersaglio o usa la mappa! <span class="yarn-meta">#line:fr01_talk_tutor_2 </span></span>
<span class="yarn-line">Arrivate il prima possibile! <span class="yarn-meta">#line:fr01_talk_tutor_3 </span></span>
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-talk-eiffell-roof"></a>
## talk_eiffell_roof

<div class="yarn-node" data-title="talk_eiffell_roof"><pre class="yarn-code"><code><span class="yarn-header-dim">group: toureiffel</span>
<span class="yarn-header-dim">tags: actor=GUIDE</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;asset toureiffell&gt;&gt;</span>
<span class="yarn-line">La Torre Eiffel è alta 300 metri. <span class="yarn-meta">#line:08c1973 </span></span>
<span class="yarn-cmd">&lt;&lt;asset mr_eiffel&gt;&gt;</span>
<span class="yarn-line">Costruito dal signor Eiffel nel 1887. <span class="yarn-meta">#line:09e5c3b </span></span>
<span class="yarn-cmd">&lt;&lt;asset iron&gt;&gt;</span>
<span class="yarn-line">È fatto di ferro! <span class="yarn-meta">#line:0d59ade </span></span>
<span class="yarn-line">Ho visto Antura dirigersi verso Notre Dame. <span class="yarn-meta">#line:04d1e52 </span></span>
<span class="yarn-cmd">&lt;&lt;camera_focus notredame&gt;&gt;</span>
<span class="yarn-line">Arrivate! <span class="yarn-meta">#line:083b3bf </span></span>
[MISSING TRANSLATION: ]
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-talk-eiffell-guide"></a>
## talk_eiffell_guide

<div class="yarn-node" data-title="talk_eiffell_guide"><pre class="yarn-code"><code><span class="yarn-header-dim">group: toureiffel</span>
<span class="yarn-header-dim">actor: guide</span>
<span class="yarn-header-dim">---</span>
&lt;&lt;if $TOTAL_COINS &gt; 2&gt;&gt;
<span class="yarn-line">    Ecco il tuo biglietto. <span class="yarn-meta">#line:04e74ad </span></span>
    <span class="yarn-cmd">&lt;&lt;asset tour_eiffell_ticket&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $TOTAL_COINS = $TOTAL_COINS-3&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;action area_toureiffel&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;asset tour_eiffell_map&gt;&gt;</span>
<span class="yarn-line">    Ho visto Antura salire in cima alla torre. <span class="yarn-meta">#line:089abda </span></span>
<span class="yarn-line">    Prendi l'ascensore! <span class="yarn-meta">#line:0585a5e </span></span>
&lt;&lt;elseif $TOTAL_COINS &gt; 0&gt;&gt; 
<span class="yarn-line">    Raccogli tutte le monete! <span class="yarn-meta">#line:04d966b </span></span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Il biglietto per la Torre Eiffel costa 3 monete. <span class="yarn-meta">#line:069cbb3 </span></span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-talk-notre-dame"></a>
## talk_notre_dame

<div class="yarn-node" data-title="talk_notre_dame"><pre class="yarn-code"><code><span class="yarn-header-dim">group: notredame</span>
<span class="yarn-header-dim">tags: actor=MAN_OLD, asset=notredame</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Sono il sindaco di Parigi. <span class="yarn-meta">#line:0cc11fa </span></span>
<span class="yarn-line">Questa è la cattedrale di Notre-Dame. <span class="yarn-meta">#line:06f3fa2 </span></span>
<span class="yarn-cmd">&lt;&lt;card notredame zoom&gt;&gt;</span>
<span class="yarn-line">È una famosa chiesa gotica, costruita nel 1182. <span class="yarn-meta">#line:02edc0f </span></span>
<span class="yarn-cmd">&lt;&lt;action AREA_NOTREDAME_ROOF&gt;&gt;</span>
<span class="yarn-line">Vieni con me sul tetto della chiesa! <span class="yarn-meta">#line:083dfcc </span></span>
<span class="yarn-cmd">&lt;&lt;set $MET_MAJOR = true&gt;&gt;</span>
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-talk-cook"></a>
## talk_cook

<div class="yarn-node" data-title="talk_cook"><pre class="yarn-code"><code><span class="yarn-header-dim">group: bakery</span>
<span class="yarn-header-dim">actor: CRAZY_MAN</span>
<span class="yarn-header-dim">---</span>
&lt;&lt;if $COLLECTED_ITEMS &gt;= 4&gt;&gt;
    <span class="yarn-cmd">&lt;&lt;jump the_end&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Aiutatemi! Antura ha fatto un pasticcio nella mia cucina! <span class="yarn-meta">#line:07bbb10 </span></span>
<span class="yarn-line">    Non riesco a trovare gli ingredienti per preparare la baguette. <span class="yarn-meta">#line:09e867c </span></span>
    <span class="yarn-cmd">&lt;&lt;asset  baguette&gt;&gt;</span>
<span class="yarn-line">    Il nostro pane francese speciale! <span class="yarn-meta">#line:0874503 </span></span>
    <span class="yarn-cmd">&lt;&lt;set $QUEST_ITEMS = 4&gt;&gt;</span>
<span class="yarn-line">    Per favore portami 4 ingredienti: <span class="yarn-meta">#line:07d64c7 </span></span>
<span class="yarn-line">    farina, acqua, lievito e sale. <span class="yarn-meta">#line:0c01530 </span></span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>
[MISSING TRANSLATION: ]
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-visit-louvre"></a>
## visit_louvre

<div class="yarn-node" data-title="visit_louvre"><pre class="yarn-code"><code><span class="yarn-header-dim">group: louvre</span>
<span class="yarn-header-dim">tags: actor=WOMAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;asset louvre_inside&gt;&gt;</span>
<span class="yarn-line">Si possono trovare numerose sculture e dipinti. <span class="yarn-meta">#line:08dc97f </span></span>
<span class="yarn-cmd">&lt;&lt;jump find_monalisa&gt;&gt;</span>
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-find-monalisa"></a>
## find_monalisa

<div class="yarn-node" data-title="find_monalisa"><pre class="yarn-code"><code><span class="yarn-header-dim">group: louvre</span>
<span class="yarn-header-dim">tags: actor=WOMAN</span>
<span class="yarn-header-dim">---</span>
[MISSING TRANSLATION: ]
<span class="yarn-cmd">&lt;&lt;action monalisa&gt;&gt;</span>
<span class="yarn-line">Vai a trovare la Monna Lisa! <span class="yarn-meta">#line:0442392 </span></span>
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-go-bakery"></a>
## go_bakery

<div class="yarn-node" data-title="go_bakery"><pre class="yarn-code"><code><span class="yarn-header-dim">group: louvre</span>
<span class="yarn-header-dim">tags: actor=WOMAN_OLD</span>
<span class="yarn-header-dim">---</span>
 [MISSING TRANSLATION: ]
<span class="yarn-line">Ora cerca Antura! È andata in panetteria a prendere una baguette! <span class="yarn-meta">#line:076ef0f </span></span>
<span class="yarn-line">Affrettarsi! <span class="yarn-meta">#line:0e9c3e7 </span></span>
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-baguette-salt"></a>
## baguette_salt

<div class="yarn-node" data-title="baguette_salt"><pre class="yarn-code" style="--node-color:yellow"><code><span class="yarn-header-dim">group: bakery</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">tags: actor=MAN_BIG</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;action COLLECT_1&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;card baguette_salt&gt;&gt;</span>
<span class="yarn-line">Questo è sale. <span class="yarn-meta">#line:00f1d2f </span></span>
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-baguette-flour"></a>
## baguette_flour

<div class="yarn-node" data-title="baguette_flour"><pre class="yarn-code" style="--node-color:yellow"><code><span class="yarn-header-dim">group: bakery</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">tags: actor=MAN_BIG</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;action COLLECT_2&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;card baguette_flour&gt;&gt;</span>
<span class="yarn-line">Questa è farina. <span class="yarn-meta">#line:06022b0 </span></span>
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-baguette-water"></a>
## baguette_water

<div class="yarn-node" data-title="baguette_water"><pre class="yarn-code" style="--node-color:yellow"><code><span class="yarn-header-dim">group: bakery</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">tags: actor=MAN_BIG</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;action COLLECT_3&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;card baguette_water&gt;&gt;</span>
<span class="yarn-line">Questa è acqua. <span class="yarn-meta">#line:0c4d1f6 </span></span>
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-baguette-yeast"></a>
## baguette_yeast

<div class="yarn-node" data-title="baguette_yeast"><pre class="yarn-code" style="--node-color:yellow"><code><span class="yarn-header-dim">group: bakery</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">tags: actor=MAN_BIG</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;action COLLECT_4&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;card baguette_yeast&gt;&gt;</span>
<span class="yarn-line">Questo è lievito. <span class="yarn-meta">#line:025865d </span></span>
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-talk-notre-dame-roof"></a>
## talk_notre_dame_roof

<div class="yarn-node" data-title="talk_notre_dame_roof"><pre class="yarn-code"><code><span class="yarn-header-dim">group: notredame</span>
<span class="yarn-header-dim">tags: actor=MAN_OLD</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;asset notredame_fire&gt;&gt;</span>
<span class="yarn-line">Nel 2019 c'è stato un grande incendio, ma siamo riusciti a ripararlo. <span class="yarn-meta">#line:09a0ead </span></span>
<span class="yarn-line">Ho visto Antura correre al Museo del Louvre. <span class="yarn-meta">#line:02ba888 </span></span>
<span class="yarn-line">Si trova proprio dall'altra parte della Senna. <span class="yarn-meta">#line:00d22e5 </span></span>
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-talk-louvre-guide"></a>
## talk_louvre_guide

<div class="yarn-node" data-title="talk_louvre_guide"><pre class="yarn-code" style="--node-color:blue"><code><span class="yarn-header-dim">group: louvre</span>
<span class="yarn-header-dim">tags: actor=WOMAN</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Benvenuti al Museo del Louvre. Cosa volete fare? <span class="yarn-meta">#line:0e6d2a5 </span></span>
<span class="yarn-line">Parlami del Louvre <span class="yarn-meta">#line:0a5fc63 </span></span>
    <span class="yarn-cmd">&lt;&lt;jump visit_louvre&gt;&gt;</span>
<span class="yarn-line">Uscita <span class="yarn-meta">#line:0efc18f </span></span>
    <span class="yarn-cmd">&lt;&lt;if $MET_MONALISA&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;action AREA_LOUVRE_EXIT&gt;&gt;</span>
<span class="yarn-line">        Ritorno! <span class="yarn-meta">#line:07dd921 </span></span>
    <span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;jump find_monalisa&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-louvre-monalisa"></a>
## louvre_monalisa

<div class="yarn-node" data-title="louvre_monalisa"><pre class="yarn-code" style="--node-color:yellow"><code><span class="yarn-header-dim">group: louvre</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">tags: actor=WOMAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;asset monalisa&gt;&gt;</span>
<span class="yarn-line">[MISSING TRANSLATION: This is the famous Mona Lisa. nalisa_1] <span class="yarn-meta">#line:louvre_monalisa_1</span></span>
<span class="yarn-cmd">&lt;&lt;set $MET_MONALISA = true&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;asset leaonardodavinci&gt;&gt;</span>
<span class="yarn-line">[MISSING TRANSLATION: It was painted around 1500 nalisa_2] <span class="yarn-meta">#line:louvre_monalisa_2</span></span>
<span class="yarn-line">[MISSING TRANSLATION: by the artist and scientist Leonardo da Vinci. nalisa_3] <span class="yarn-meta">#line:louvre_monalisa_3</span></span>
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-louvre-liberty"></a>
## louvre_liberty

<div class="yarn-node" data-title="louvre_liberty"><pre class="yarn-code" style="--node-color:yellow"><code><span class="yarn-header-dim">group: louvre</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">tags: actor=WOMAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;asset liberty_leading&gt;&gt;</span>
<span class="yarn-line">[MISSING TRANSLATION: This painting represents freedom.] <span class="yarn-meta">#line:louvre_liberty_1</span></span>
<span class="yarn-line">[MISSING TRANSLATION: It's called Liberty Leading the People] <span class="yarn-meta">#line:louvre_liberty_2</span></span>
<span class="yarn-line">[MISSING TRANSLATION: by the French artist Eugène Delacroix] <span class="yarn-meta">#line:louvre_liberty_3</span></span>
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-louvre-venus"></a>
## louvre_venus

<div class="yarn-node" data-title="louvre_venus"><pre class="yarn-code" style="--node-color:yellow"><code><span class="yarn-header-dim">group: louvre</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">tags: actor=WOMAN, </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;asset venusmilo&gt;&gt;</span>
<span class="yarn-line">La Venere di Milo, antica scultura greca in marmo. <span class="yarn-meta">#line:053d4fe </span></span>
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-talk-louvre-external"></a>
## talk_louvre_external

<div class="yarn-node" data-title="talk_louvre_external"><pre class="yarn-code"><code><span class="yarn-header-dim">group: louvre</span>
<span class="yarn-header-dim">actor: OLD_WOMAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $MET_MONALISA&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump go_bakery&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;asset louvre&gt;&gt;</span>
<span class="yarn-line">donna: Questo è l'ingresso del Louvre, il nostro museo d'arte nazionale. <span class="yarn-meta">#line:0cf1cc8 </span></span>
<span class="yarn-line">donna: Vuoi entrare? <span class="yarn-meta">#line:0f74ff9</span></span>
<span class="yarn-line">SÌ <span class="yarn-meta">#line:090114f </span></span>
<span class="yarn-line">    Buona visita! <span class="yarn-meta">#line:056e051 </span></span>
    <span class="yarn-cmd">&lt;&lt;action AREA_LOUVRE_ENTER &gt;&gt;</span>
<span class="yarn-line">NO <span class="yarn-meta">#line:077422a </span></span>
<span class="yarn-line">    Va bene. <span class="yarn-meta">#line:0c28ea0 </span></span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-spawned-woman"></a>
## spawned_woman

<div class="yarn-node" data-title="spawned_woman"><pre class="yarn-code" style="--node-color:purple"><code><span class="yarn-header-dim">///////// NPCs SPAWNED IN THE SCENE //////////</span>
<span class="yarn-header-dim">// these npc are spawn automatically in the scene</span>
<span class="yarn-header-dim">// use these to add random facts. everythime you meet them</span>
<span class="yarn-header-dim">// they will say one of these lines randomly</span>
<span class="yarn-header-dim">tags: actor=WOMAN</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">spawn_group: generic</span>

<span class="yarn-header-dim">---</span>
<span class="yarn-line">Ciao. Cosa vuoi sapere? <span class="yarn-meta">#line:0070084 </span></span>
<span class="yarn-line">Cos'è la Torre Eiffel? <span class="yarn-meta">#line:0d91dc0 </span></span>
<span class="yarn-line">    La famosa torre di ferro, alta 300 metri. <span class="yarn-meta">#line:0f17af0 </span></span>
<span class="yarn-line">    Il simbolo di Parigi! <span class="yarn-meta">#line:07a113f </span></span>
<span class="yarn-line">Dove siamo? <span class="yarn-meta">#line:09dd1da </span></span>
<span class="yarn-line">    Siamo a Parigi, la città dell'amore! <span class="yarn-meta">#line:02b627d </span></span>
<span class="yarn-line">Questo posto è reale? <span class="yarn-meta">#line:08bede4 </span></span>
<span class="yarn-line">    Certo! Perché me lo chiedi? <span class="yarn-meta">#line:08654e6 </span></span>
<span class="yarn-line">    Beh... sembra un videogioco, non è vero? <span class="yarn-meta">#line:0bc62a3 </span></span>
<span class="yarn-line">Niente. Ciao. <span class="yarn-meta">#line:0fe0732 </span></span>
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-spawned-man"></a>
## spawned_man

<div class="yarn-node" data-title="spawned_man"><pre class="yarn-code" style="--node-color:purple"><code><span class="yarn-header-dim">tags: actor=MAN</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">spawn_group: generic</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Hai qualche domanda? <span class="yarn-meta">#line:07b94e9 </span></span>
<span class="yarn-line">Hai visto Antura? <span class="yarn-meta">#line:0f18ad3 </span></span>
<span class="yarn-line">    Sì! Parla con tutti e segui le luci! <span class="yarn-meta">#line:0cf9b4e </span></span>
<span class="yarn-line">    No. Chi è Antura? <span class="yarn-meta">#line:0f9dd62 </span></span>
<span class="yarn-line">Cosa fai? <span class="yarn-meta">#line:002796f </span></span>
<span class="yarn-line">    Vado a lavorare! <span class="yarn-meta">#line:0fe4ff4 </span></span>
<span class="yarn-line">    Vado a comprare il pane al panificio. <span class="yarn-meta">#line:05a38a8 </span></span>
<span class="yarn-line">Da dove vieni? <span class="yarn-meta">#line:05eabcf </span></span>
<span class="yarn-line">    Non sono nato in questo paese. <span class="yarn-meta">#line:0635a6a </span></span>
<span class="yarn-line">    Dal pianeta Terra. <span class="yarn-meta">#line:0749690 </span></span>
<span class="yarn-line">Arrivederci <span class="yarn-meta">#line:0ee51fc </span></span>
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-npc-kid"></a>
## npc_kid

<div class="yarn-node" data-title="npc_kid"><pre class="yarn-code" style="--node-color:purple"><code><span class="yarn-header-dim">tags: actor=KID_M</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">spawn_group: kids</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">CIAO! <span class="yarn-meta">#line:0c4d9e4 </span></span>
<span class="yarn-line">Come stai? <span class="yarn-meta">#line:032d401 </span></span>
[MISSING TRANSLATION: ]
</code></pre></div>


