---
title: Paris ! (fr_01) - Script
hide:
---

# Paris ! (fr_01) - Script
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

<span class="yarn-line">Bienvenue à Paris !</span> <span class="yarn-meta">#line:start </span>
<span class="yarn-line">Va parler au tuteur !</span> <span class="yarn-meta">#line:start_2</span>
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
<span class="yarn-line">Super ! Je peux maintenant faire cuire la baguette. Et…</span> <span class="yarn-meta">#line:0017917 </span>
<span class="yarn-line">FÉLICITATIONS ! Vous avez gagné ! Vous avez aimé ?</span> <span class="yarn-meta">#line:0d11596 </span>
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
<span class="yarn-line">Pourquoi ne dessines-tu pas la Tour Eiffel ?</span> <span class="yarn-meta">#line:002620f </span>
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
<span class="yarn-line">J'ai vu Antura aller à la Tour Eiffel.</span> <span class="yarn-meta">#line:talk_tutor</span>
<span class="yarn-cmd">&lt;&lt;camera_focus tour_eiffell&gt;&gt;</span>
<span class="yarn-line">Suivez la lumière ou utilisez la carte !</span> <span class="yarn-meta">#line:talk_tutor_2 </span>
<span class="yarn-line">Allez-y maintenant !</span> <span class="yarn-meta">#line:talk_tutor_3 </span>

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
<span class="yarn-line">La tour Eiffel mesure 300 mètres de haut.</span> <span class="yarn-meta">#line:08c1973 </span>
<span class="yarn-cmd">&lt;&lt;asset mr_eiffel&gt;&gt;</span>
<span class="yarn-line">Construit par Monsieur Eiffel en 1887.</span> <span class="yarn-meta">#line:09e5c3b </span>
<span class="yarn-cmd">&lt;&lt;asset iron&gt;&gt;</span>
<span class="yarn-line">C'est fait de fer !</span> <span class="yarn-meta">#line:0d59ade </span>
<span class="yarn-line">J'ai vu Antura se diriger vers Notre Dame.</span> <span class="yarn-meta">#line:04d1e52 </span>
<span class="yarn-cmd">&lt;&lt;camera_focus notredame&gt;&gt;</span>
<span class="yarn-line">Allez-y !</span> <span class="yarn-meta">#line:083b3bf </span>


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
<span class="yarn-line">    Voici votre billet.</span> <span class="yarn-meta">#line:04e74ad </span>
    <span class="yarn-cmd">&lt;&lt;asset tour_eiffell_ticket&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $TOTAL_COINS = $TOTAL_COINS-3&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;action area_toureiffel&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;asset tour_eiffell_map&gt;&gt;</span>
<span class="yarn-line">    J'ai vu Antura monter au sommet de la tour.</span> <span class="yarn-meta">#line:089abda </span>
<span class="yarn-line">    Prenez l'ascenseur !</span> <span class="yarn-meta">#line:0585a5e </span>
&lt;&lt;elseif $TOTAL_COINS &gt; 0&gt;&gt; 
<span class="yarn-line">    Collectez toutes les pièces !</span> <span class="yarn-meta">#line:04d966b </span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Le billet pour la Tour Eiffel coûte 3 pièces.</span> <span class="yarn-meta">#line:069cbb3 </span>
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
<span class="yarn-line">Je suis le maire de Paris.</span> <span class="yarn-meta">#line:0cc11fa </span>
<span class="yarn-line">C'est la cathédrale Notre-Dame.</span> <span class="yarn-meta">#line:06f3fa2 </span>
<span class="yarn-cmd">&lt;&lt;card notredame zoom&gt;&gt;</span>
<span class="yarn-line">C'est une célèbre église gothique, construite en 1182.</span> <span class="yarn-meta">#line:02edc0f </span>
<span class="yarn-cmd">&lt;&lt;action AREA_NOTREDAME_ROOF&gt;&gt;</span>
<span class="yarn-line">Viens avec moi sur le toit de l'église !</span> <span class="yarn-meta">#line:083dfcc </span>
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
<span class="yarn-line">Il y a eu un gros incendie en 2019, mais nous avons pu le réparer.</span> <span class="yarn-meta">#line:09a0ead </span>
<span class="yarn-line">J'ai vu Antura courir dans le musée du Louvre.</span> <span class="yarn-meta">#line:02ba888 </span>
<span class="yarn-line">C'est juste de l'autre côté de la Seine.</span> <span class="yarn-meta">#line:00d22e5 </span>

</code>
</pre>
</div>

<a id="ys-node-gargoyle"></a>

## gargoyle

<div class="yarn-node" data-title="gargoyle">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: notredame</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Regardez cette statue !</span> <span class="yarn-meta">#line:0f7f9d8 </span>
<span class="yarn-cmd">&lt;&lt;card gargoyle zoom&gt;&gt;</span>
<span class="yarn-line">N'est-ce pas effrayant ?</span> <span class="yarn-meta">#line:0b5d057 </span>

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
<span class="yarn-line">femme : C'est l'entrée du Louvre, notre musée d'art national.</span> <span class="yarn-meta">#line:0cf1cc8 </span>
<span class="yarn-line">femme : Tu veux entrer ?</span> <span class="yarn-meta">#line:0f74ff9</span>
<span class="yarn-line">Oui</span> <span class="yarn-meta">#line:090114f </span>
<span class="yarn-line">    Bonne visite !</span> <span class="yarn-meta">#line:056e051 </span>
    <span class="yarn-cmd">&lt;&lt;action AREA_LOUVRE_ENTER &gt;&gt;</span>
<span class="yarn-line">Non</span> <span class="yarn-meta">#line:077422a </span>
<span class="yarn-line">    D'accord.</span> <span class="yarn-meta">#line:0c28ea0 #do_not_translate</span>
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
<span class="yarn-line">Bienvenue au musée du Louvre. Que souhaitez-vous faire ?</span> <span class="yarn-meta">#line:0e6d2a5 </span>
<span class="yarn-line">Parlez-moi du Louvre</span> <span class="yarn-meta">#line:0a5fc63 </span>
    <span class="yarn-cmd">&lt;&lt;jump visit_louvre&gt;&gt;</span>
<span class="yarn-line">Sortie</span> <span class="yarn-meta">#line:0efc18f </span>
    <span class="yarn-cmd">&lt;&lt;if $MET_MONALISA&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;action AREA_LOUVRE_EXIT&gt;&gt;</span>
<span class="yarn-line">        Revenir!</span> <span class="yarn-meta">#line:07dd921 </span>
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
<span class="yarn-line">C'est la célèbre Mona Lisa.</span> <span class="yarn-meta">#line:louvre_monalisa_1</span>
<span class="yarn-cmd">&lt;&lt;set $MET_MONALISA = true&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;asset leaonardodavinci&gt;&gt;</span>
<span class="yarn-line">Léonard l'a peint vers 1500.</span> <span class="yarn-meta">#line:louvre_monalisa_2</span>
<span class="yarn-line">par l'artiste Léonard de Vinci.</span> <span class="yarn-meta">#line:louvre_monalisa_3</span>

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
<span class="yarn-line">Ce tableau représente la liberté.</span> <span class="yarn-meta">#line:louvre_liberty_1</span>
<span class="yarn-line">Cela s'appelle La Liberté guidant le peuple.</span> <span class="yarn-meta">#line:louvre_liberty_2</span>
<span class="yarn-line">par l'artiste français Eugène Delacroix.</span> <span class="yarn-meta">#line:louvre_liberty_3</span>

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
<span class="yarn-line">La Vénus de Milo, une sculpture en marbre de la Grèce antique.</span> <span class="yarn-meta">#line:053d4fe </span>

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
<span class="yarn-line">Cette pyramide de verre est l'entrée principale du musée.</span> <span class="yarn-meta">#line:fr01_pyramid_1</span>
<span class="yarn-line">Il a été construit dans les années 1980 pour accueillir davantage de visiteurs.</span> <span class="yarn-meta">#line:fr01_pyramid_2</span>

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
<span class="yarn-line">Cette pierre possède de très anciennes lois de l'ancienne Mésopotamie.</span> <span class="yarn-meta">#line:fr01_hammurabi_1</span>
<span class="yarn-line">Ils ont été écrits il y a près de 4 000 ans.</span> <span class="yarn-meta">#line:fr01_hammurabi_2</span>

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
<span class="yarn-line">Ce grand tableau montre Napoléon devenant empereur.</span> <span class="yarn-meta">#line:fr01_coronation_1</span>
<span class="yarn-line">L'artiste Jacques-Louis David a peint de nombreux détails.</span> <span class="yarn-meta">#line:fr01_coronation_2</span>

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
<span class="yarn-line">Ce tableau montre des frères faisant une promesse courageuse.</span> <span class="yarn-meta">#line:fr01_horatii_1</span>
<span class="yarn-line">Il enseigne le devoir et le courage depuis la Rome antique.</span> <span class="yarn-meta">#line:fr01_horatii_2</span>

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
<span class="yarn-line">Cette statue représente un homme en train d'écrire dans l'Égypte ancienne.</span> <span class="yarn-meta">#line:fr01_scribe_1</span>
<span class="yarn-line">Ses yeux semblent très réels et brillants.</span> <span class="yarn-meta">#line:fr01_scribe_2</span>

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
<span class="yarn-line">Cette statue représente une figure ailée atterrissant sur un navire.</span> <span class="yarn-meta">#line:fr01_victory_1</span>
<span class="yarn-line">Le vent façonne ses vêtements et ses ailes.</span> <span class="yarn-meta">#line:fr01_victory_2</span>

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
<span class="yarn-line">    Au secours ! Antura a mis le bazar dans ma cuisine !</span> <span class="yarn-meta">#line:07bbb10 </span>
<span class="yarn-line">    Je ne trouve pas les ingrédients pour faire la baguette.</span> <span class="yarn-meta">#line:09e867c </span>
    <span class="yarn-cmd">&lt;&lt;asset  baguette&gt;&gt;</span>
<span class="yarn-line">    Notre pain français spécial !</span> <span class="yarn-meta">#line:0874503 </span>
    <span class="yarn-cmd">&lt;&lt;set $QUEST_ITEMS = 4&gt;&gt;</span>
<span class="yarn-line">    S'il vous plaît, apportez-moi 4 ingrédients :</span> <span class="yarn-meta">#line:07d64c7 </span>
<span class="yarn-line">    farine, eau, levure et sel.</span> <span class="yarn-meta">#line:0c01530 </span>
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
<span class="yarn-line">Vous pourrez y découvrir de nombreuses sculptures et peintures.</span> <span class="yarn-meta">#line:08dc97f </span>
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
<span class="yarn-line">Allez chercher la Joconde !</span> <span class="yarn-meta">#line:0442392 </span>

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
 
<span class="yarn-line">Maintenant, cherchez Antura ! Elle est allée à la boulangerie chercher une baguette !</span> <span class="yarn-meta">#line:076ef0f </span>
<span class="yarn-line">Dépêche-toi!</span> <span class="yarn-meta">#line:0e9c3e7 </span>

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
<span class="yarn-line">C'est du sel.</span> <span class="yarn-meta">#line:00f1d2f </span>

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
<span class="yarn-line">C'est de la farine.</span> <span class="yarn-meta">#line:06022b0 </span>

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
<span class="yarn-line">C'est de l'eau.</span> <span class="yarn-meta">#line:0c4d1f6 </span>

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
<span class="yarn-line">C'est de la levure.</span> <span class="yarn-meta">#line:025865d </span>

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
<span class="yarn-line">Salut. Que veux-tu savoir ?</span> <span class="yarn-meta">#line:0070084 </span>
<span class="yarn-line">Qu'est-ce que la Tour Eiffel ?</span> <span class="yarn-meta">#line:0d91dc0 </span>
<span class="yarn-line">    La célèbre tour de fer, haute de 300 mètres.</span> <span class="yarn-meta">#line:0f17af0 </span>
<span class="yarn-line">    Le symbole de Paris !</span> <span class="yarn-meta">#line:07a113f </span>
<span class="yarn-line">Où sommes-nous?</span> <span class="yarn-meta">#line:09dd1da </span>
<span class="yarn-line">    Nous sommes à Paris, la ville de l'amour !</span> <span class="yarn-meta">#line:02b627d </span>
<span class="yarn-line">Cet endroit est-il réel ?</span> <span class="yarn-meta">#line:08bede4 </span>
<span class="yarn-line">    Bien sûr ! Pourquoi demandes-tu ça ?</span> <span class="yarn-meta">#line:08654e6 </span>
<span class="yarn-line">    Eh bien... ça ressemble à un jeu vidéo, n'est-ce pas ?</span> <span class="yarn-meta">#line:0bc62a3 </span>
<span class="yarn-line">Rien. Au revoir.</span> <span class="yarn-meta">#line:0fe0732 </span>

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
<span class="yarn-line">J'aimerais monter sur la Tour Eiffel.</span> <span class="yarn-meta">#line:0aee9bb </span>
<span class="yarn-line">Il faut un ticket pour monter.</span> <span class="yarn-meta">#line:09be864 </span>
<span class="yarn-line">Il y a eu une grande foire à Paris en 1889.</span> <span class="yarn-meta">#line:0a3f4e1 </span>
<span class="yarn-line">    C'était pour célébrer le 100e anniversaire de la Révolution française.</span> <span class="yarn-meta">#line:01fa210 </span>
<span class="yarn-line">    La Tour Eiffel a été construite pour cet événement.</span> <span class="yarn-meta">#line:0d6f3c4 </span>
<span class="yarn-line">J'adore Paris !</span> <span class="yarn-meta">#line:0bda18a </span>

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
<span class="yarn-line">Avez-vous des questions?</span> <span class="yarn-meta">#line:07b94e9 </span>
<span class="yarn-line">Avez-vous vu Antura ?</span> <span class="yarn-meta">#line:0f18ad3 </span>
<span class="yarn-line">    Oui ! Parlez à tout le monde et suivez les lumières !</span> <span class="yarn-meta">#line:0cf9b4e </span>
<span class="yarn-line">    Non. Qui est Antura ?</span> <span class="yarn-meta">#line:0f9dd62 </span>
<span class="yarn-line">Que fais-tu?</span> <span class="yarn-meta">#line:002796f </span>
<span class="yarn-line">    Je vais au travail !</span> <span class="yarn-meta">#line:0fe4ff4 </span>
<span class="yarn-line">    Je vais acheter du pain à la boulangerie.</span> <span class="yarn-meta">#line:05a38a8 </span>
<span class="yarn-line">D'où viens-tu?</span> <span class="yarn-meta">#line:05eabcf </span>
<span class="yarn-line">    Je ne suis pas né dans ce pays.</span> <span class="yarn-meta">#line:0635a6a </span>
<span class="yarn-line">    De la planète Terre.</span> <span class="yarn-meta">#line:0749690 </span>
<span class="yarn-line">Au revoir</span> <span class="yarn-meta">#line:0ee51fc </span>

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
<span class="yarn-line">Salut!</span> <span class="yarn-meta">#line:0c4d9e4 </span>
<span class="yarn-line">Comment vas-tu?</span> <span class="yarn-meta">#line:032d401 </span>

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
<span class="yarn-line">Bonjour!</span> <span class="yarn-meta">#line:041403d </span>
<span class="yarn-line">Ça va ?</span> <span class="yarn-meta">#line:04986a3 </span>

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
<span class="yarn-line">Le Louvre est l’un des plus grands musées du monde.</span> <span class="yarn-meta">#line:fr01_louvre_rand_1</span>
<span class="yarn-line">Vous pouvez marcher ici pendant des heures sans toujours tout voir.</span> <span class="yarn-meta">#line:fr01_louvre_rand_2</span>
<span class="yarn-line">De nombreuses œuvres d’art ici sont plus anciennes que les grands-parents de vos grands-parents.</span> <span class="yarn-meta">#line:fr01_louvre_rand_3</span>
<span class="yarn-line">La pyramide de verre laisse entrer la lumière dans les salles situées en dessous.</span> <span class="yarn-meta">#line:fr01_louvre_rand_4</span>

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
<span class="yarn-line">Notre-Dame est une célèbre cathédrale gothique.</span> <span class="yarn-meta">#line:fr01_notredame_base_1</span>
<span class="yarn-line">Les constructeurs ont commencé à construire ce bâtiment il y a plus de 800 ans.</span> <span class="yarn-meta">#line:fr01_notredame_base_2</span>
<span class="yarn-line">Les grosses cloches sonnent dans toute la ville.</span> <span class="yarn-meta">#line:fr01_notredame_base_3</span>

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
<span class="yarn-line">Depuis le toit, vous pouvez voir une grande partie de Paris.</span> <span class="yarn-meta">#line:fr01_notredame_roof_1</span>
<span class="yarn-line">Des créatures de pierre appelées gargouilles sont assises ici.</span> <span class="yarn-meta">#line:fr01_notredame_roof_2</span>
<span class="yarn-line">Les ouvriers restaurent encore certaines parties de la cathédrale.</span> <span class="yarn-meta">#line:fr01_notredame_roof_3</span>

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
<span class="yarn-line">L’odeur du pain frais rend les gens heureux.</span> <span class="yarn-meta">#line:fr01_bakery_1</span>
<span class="yarn-line">Une baguette utilise de la farine, de l’eau, de la levure et du sel.</span> <span class="yarn-meta">#line:fr01_bakery_2</span>
<span class="yarn-line">Les boulangers se lèvent très tôt pour commencer à faire la pâte.</span> <span class="yarn-meta">#line:fr01_bakery_3</span>

</code>
</pre>
</div>


