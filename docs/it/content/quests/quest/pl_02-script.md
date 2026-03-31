---
title: Il grande salvataggio dei nani di Breslavia (pl_02) - Script
hide:
---

# Il grande salvataggio dei nani di Breslavia (pl_02) - Script
> [!note] Educators & Designers: help improving this quest!
> **Comments and feedback**: [discuss in the Forum](https://antura.discourse.group/t/pl-02-the-great-wroclaw-dwarf-rescue/33/1)  
> **Improve script translations**: [comment the Google Sheet](https://docs.google.com/spreadsheets/d/1FPFOy8CHor5ArSg57xMuPAG7WM27-ecDOiU-OmtHgjw/edit?gid=1721014062#gid=1721014062)  
> **Improve Cards translations**: [comment the Google Sheet](https://docs.google.com/spreadsheets/d/1M3uOeqkbE4uyDs5us5vO-nAFT8Aq0LGBxjjT_CSScWw/edit?gid=415931977#gid=415931977)  
> **Improve the script**: [propose an edit here](https://github.com/vgwb/Antura/blob/main/Assets/_discover/_quests/PL_02%20Wroclaw%20Dwarves/PL_02%20Wroclaw%20Dwarves%20-%20Yarn%20Script.yarn)  

<a id="ys-node-quest-start"></a>

## quest_start

<div class="yarn-node" data-title="quest_start">
<pre class="yarn-code" style="--node-color:red"><code>
<span class="yarn-header-dim">// pl_02 | Dwarves (Wroclaw)</span>
<span class="yarn-header-dim">// </span>
<span class="yarn-header-dim">type: panel</span>
<span class="yarn-header-dim">color: red</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;declare $current_place = "school"&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $dwarf_0_done = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $dwarf_1_done = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $dwarf_2_done = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $dwarf_3_done = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $dwarf_4_done = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $dwarf_5_done = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $dwarf_6_done = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $dwarf_7_done = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $dwarf_8_done = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $dwarf_9_done = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $dwarf_10_done = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $area_center_done = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $area_centennial_done = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $secret_0_done = false&gt;&gt;</span>

<span class="yarn-comment">// hide all the keys</span>
<span class="yarn-cmd">&lt;&lt;SetActive key_0 false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;SetActive key_1 false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;SetActive key_2 false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;SetActive key_3 false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;SetActive key_4 false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;SetActive key_5 false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;SetActive key_6 false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;SetActive key_7 false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;SetActive key_8 false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;SetActive key_9 false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;SetActive key_10 false&gt;&gt;</span>

<span class="yarn-comment">// DEBUG (comment in production)</span>
<span class="yarn-comment">//&lt;&lt;set $dwarf_0_done = true&gt;&gt;</span>
<span class="yarn-comment">//&lt;&lt;set $dwarf_1_done = true&gt;&gt;</span>
<span class="yarn-comment">//&lt;&lt;set $dwarf_2_done = true&gt;&gt;</span>
<span class="yarn-comment">//&lt;&lt;set $dwarf_3_done = true&gt;&gt;</span>
<span class="yarn-comment">//&lt;&lt;set $dwarf_4_done = true&gt;&gt;</span>
<span class="yarn-comment">//&lt;&lt;set $dwarf_5_done = true&gt;&gt;</span>
<span class="yarn-comment">//&lt;&lt;set $dwarf_6_done = true&gt;&gt;</span>
<span class="yarn-comment">//&lt;&lt;set $dwarf_7_done = true&gt;&gt;</span>
<span class="yarn-comment">//&lt;&lt;set $dwarf_8_done = true&gt;&gt;</span>
<span class="yarn-comment">//&lt;&lt;set $dwarf_9_done = true&gt;&gt;</span>
<span class="yarn-comment">//&lt;&lt;set $dwarf_10_done = true&gt;&gt;</span>
<span class="yarn-comment">//&lt;&lt;jump tram&gt;&gt;</span>

<span class="yarn-cmd">&lt;&lt;card wroclaw&gt;&gt;</span>
<span class="yarn-line">Benvenuti a Breslavia!</span> <span class="yarn-meta">#line:082706f </span>
<span class="yarn-line">C'è un problema in città... andiamo a scoprire di cosa si tratta!</span> <span class="yarn-meta">#line:029d26c </span>
<span class="yarn-cmd">&lt;&lt;jump intro_area_school&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-quest-end"></a>

## quest_end

<div class="yarn-node" data-title="quest_end">
<pre class="yarn-code" style="--node-color:red"><code>
<span class="yarn-header-dim">type: panel_endgame</span>
<span class="yarn-header-dim">color: red</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Hai completato questa missione. Ti è piaciuta?</span> <span class="yarn-meta">#line:0069d94 </span>
<span class="yarn-cmd">&lt;&lt;jump post_quest_activity&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-post-quest-activity"></a>

## post_quest_activity

<div class="yarn-node" data-title="post_quest_activity">
<pre class="yarn-code" style="--node-color:red"><code>
<span class="yarn-header-dim">type: panel</span>
<span class="yarn-header-dim">tags: proposal</span>
<span class="yarn-header-dim">color: red</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Disegna il tuo nano di Breslavia preferito e il luogo in cui l'hai trovato.</span> <span class="yarn-meta">#line:00d7df1 </span>
<span class="yarn-cmd">&lt;&lt;quest_end&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-teleport"></a>

## teleport

<div class="yarn-node" data-title="teleport">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $area_center_done and $area_centennial_done&gt;&gt;</span>
<span class="yarn-line">    Ora puoi andare alla Sky Tower!</span> <span class="yarn-meta">#line:042161c </span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>
<span class="yarn-line">Dove vuoi andare?</span> <span class="yarn-meta">#line:where_to_go</span>
-&gt; School <span class="yarn-cmd">&lt;&lt;if $current_place == "cathedral"&gt;&gt;</span>  <span class="yarn-meta">#line:0d0e53a </span>
    <span class="yarn-cmd">&lt;&lt;teleport tram_school&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;wait 4&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump intro_area_school&gt;&gt;</span>
-&gt; Cathedral <span class="yarn-cmd">&lt;&lt;if $current_place != "cathedral"&gt;&gt;</span>  <span class="yarn-meta">#line:tram_cathedral</span>
    <span class="yarn-cmd">&lt;&lt;teleport tram_cathedral&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;wait 4&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump intro_area_cathedral&gt;&gt;</span>
-&gt; Market Hall <span class="yarn-cmd">&lt;&lt;if $current_place != "center"&gt;&gt;</span>  <span class="yarn-meta">#line:tram_market</span>
    <span class="yarn-cmd">&lt;&lt;teleport tram_center&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;wait 4&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump intro_area_center&gt;&gt;</span>
-&gt; Panorama <span class="yarn-cmd">&lt;&lt;if $current_place == "center" and !$dwarf_4_done&gt;&gt;</span>  <span class="yarn-meta">#line:01008e2 </span>
    <span class="yarn-cmd">&lt;&lt;teleport tram_panorama&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;wait 4&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump intro_area_panorama&gt;&gt;</span>
-&gt; Centennial Hall <span class="yarn-cmd">&lt;&lt;if $current_place != "centennial"&gt;&gt;</span>  <span class="yarn-meta">#line:tram_centennial</span>
    <span class="yarn-cmd">&lt;&lt;teleport tram_centennial_hall&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;wait 4&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump intro_area_centennial&gt;&gt;</span>
-&gt; Sky Tower <span class="yarn-cmd">&lt;&lt;if $area_centennial_done and $area_center_done and $current_place != "skytower"&gt;&gt;</span>  <span class="yarn-meta">#line:012413f </span>
    <span class="yarn-cmd">&lt;&lt;teleport tram_skytower&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;wait 4&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump intro_area_skytower&gt;&gt;</span>
<span class="yarn-choice">-&gt; Resta qui</span> <span class="yarn-meta">#line:stay_here #highlight</span>

</code>
</pre>
</div>

<a id="ys-node-driver-tram-school"></a>

## driver_tram_school

<div class="yarn-node" data-title="driver_tram_school">
<pre class="yarn-code" style="--node-color:blue"><code>
<span class="yarn-header-dim">group: school</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $dwarf_0_done&gt;&gt;</span>
<span class="yarn-line">Dove vuoi andare?</span> <span class="yarn-meta">#shadow:where_to_go</span>
-&gt; Cathedral <span class="yarn-cmd">&lt;&lt;if $dwarf_0_done&gt;&gt;</span>  <span class="yarn-meta">#shadow:tram_cathedral</span>
    <span class="yarn-cmd">&lt;&lt;teleport tram_cathedral&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;wait 4&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump intro_area_cathedral&gt;&gt;</span>
<span class="yarn-choice">-&gt; Resta qui</span> <span class="yarn-meta">#shadow:stay_here #highlight</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Completa prima il compito!</span> <span class="yarn-meta">#line:0beae23 </span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-driver-tram-cathedral"></a>

## driver_tram_cathedral

<div class="yarn-node" data-title="driver_tram_cathedral">
<pre class="yarn-code" style="--node-color:blue"><code>
<span class="yarn-header-dim">group: cathedral</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">type: panel</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;jump teleport&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-driver-tram-center"></a>

## driver_tram_center

<div class="yarn-node" data-title="driver_tram_center">
<pre class="yarn-code" style="--node-color:blue"><code>
<span class="yarn-header-dim">group: center</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">type: panel</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;jump teleport&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-driver-tram-panorama"></a>

## driver_tram_panorama

<div class="yarn-node" data-title="driver_tram_panorama">
<pre class="yarn-code" style="--node-color:blue"><code>
<span class="yarn-header-dim">group: panorama</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Dove vuoi andare?</span> <span class="yarn-meta">#shadow:where_to_go</span>
<span class="yarn-choice">-&gt; Mercato coperto</span> <span class="yarn-meta">#shadow:tram_market</span>
    <span class="yarn-cmd">&lt;&lt;teleport tram_center&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;wait 4&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump intro_area_center&gt;&gt;</span>
<span class="yarn-choice">-&gt; Sala del Centenario</span> <span class="yarn-meta">#shadow:tram_centennial</span>
    <span class="yarn-cmd">&lt;&lt;teleport tram_centennial_hall&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;wait 4&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump intro_area_centennial&gt;&gt;</span>
<span class="yarn-choice">-&gt; Resta qui</span> <span class="yarn-meta">#shadow:stay_here #highlight</span>

</code>
</pre>
</div>

<a id="ys-node-driver-tram-centennial"></a>

## driver_tram_centennial

<div class="yarn-node" data-title="driver_tram_centennial">
<pre class="yarn-code" style="--node-color:blue"><code>
<span class="yarn-header-dim">group: centennial</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">type: panel</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;jump teleport&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-driver-tram-skytower"></a>

## driver_tram_skytower

<div class="yarn-node" data-title="driver_tram_skytower">
<pre class="yarn-code" style="--node-color:blue"><code>
<span class="yarn-header-dim">group: skytower</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">type: panel</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;jump teleport&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-intro-area-school"></a>

## intro_area_school

<div class="yarn-node" data-title="intro_area_school">
<pre class="yarn-code" style="--node-color:blue"><code>
<span class="yarn-header-dim">// -----------------------------------------------------------------------------</span>
<span class="yarn-header-dim">// AREA SCHOOL</span>
<span class="yarn-header-dim">group: school</span>
<span class="yarn-header-dim">type: panel</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;set $current_place = "school"&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;area area_school&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;target npc_0&gt;&gt;</span>
<span class="yarn-line">Ci troviamo alla scuola elementare Leonardo!</span> <span class="yarn-meta">#line:005d84c </span>

</code>
</pre>
</div>

<a id="ys-node-npc-0"></a>

## npc_0

<div class="yarn-node" data-title="npc_0">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">group: school</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $dwarf_0_done&gt;&gt;</span>
<span class="yarn-line">    Ora sapete come liberare gli altri nani.</span> <span class="yarn-meta">#line:04b5c6e </span>
    <span class="yarn-cmd">&lt;&lt;camera_focus camera_skytower_roof&gt;&gt;</span>
<span class="yarn-line">    Antura si trova in cima alla Sky Tower!</span> <span class="yarn-meta">#line:09771a2 </span>
    <span class="yarn-cmd">&lt;&lt;camera_reset&gt;&gt;</span>
<span class="yarn-line">    Il tram è pronto. Per prima cosa, andate alla Cattedrale.</span> <span class="yarn-meta">#line:00f4741 </span>
    <span class="yarn-cmd">&lt;&lt;target driver_tram_school&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif GetCurrentTask() == "dwarf_0"&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump hint_0&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card primary_school_leonardo_da_vinci&gt;&gt;</span>
<span class="yarn-line">    Ciao! Sono un'insegnante della scuola Leonardo.</span> <span class="yarn-meta">#line:08d54cc</span>
    <span class="yarn-cmd">&lt;&lt;card wroclaw&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;camera_focus camera_town_view&gt;&gt;</span>
<span class="yarn-line">    Antura ha girato tutta Breslavia stasera!</span> <span class="yarn-meta">#line:0b6daaa</span>
    <span class="yarn-cmd">&lt;&lt;camera_reset&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card wroclaw_dwarfs&gt;&gt;</span>
<span class="yarn-line">    E stamattina, dieci nani sono stati rinchiusi in gabbie!</span> <span class="yarn-meta">#line:0b42c98</span>
<span class="yarn-line">    Ogni gabbia è chiusa con una chiave d'oro.</span> <span class="yarn-meta">#line:084b7e8 </span>
    <span class="yarn-cmd">&lt;&lt;camera_focus camera_school&gt;&gt;</span>
<span class="yarn-line">    Salvate il nano della scuola.</span> <span class="yarn-meta">#line:0294a8c </span>
    <span class="yarn-cmd">&lt;&lt;camera_reset&gt;&gt;</span>
<span class="yarn-line">    Parlami se hai bisogno di aiuto.</span> <span class="yarn-meta">#line:talk_4_help</span>
    <span class="yarn-cmd">&lt;&lt;task_start dwarf_0&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;target dwarf_0&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;SetActive key_0 true&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-info-0"></a>

## info_0

<div class="yarn-node" data-title="info_0">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">group: school</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card primary_school_leonardo_da_vinci&gt;&gt;</span>
<span class="yarn-line">Ti piace andare a scuola?</span> <span class="yarn-meta">#line:info_00</span>
<span class="yarn-choice">-&gt; SÌ</span> <span class="yarn-meta">#shadow:yes</span>
    <span class="yarn-cmd">&lt;&lt;if !$secret_0_done&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;set $secret_0_done = true&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;collect cookie&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>
<span class="yarn-choice">-&gt; NO</span> <span class="yarn-meta">#shadow:no</span>

</code>
</pre>
</div>

<a id="ys-node-hint-0"></a>

## hint_0

<div class="yarn-node" data-title="hint_0">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: school</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Come posso essere d'aiuto?</span> <span class="yarn-meta">#line:how_can_help </span>
<span class="yarn-choice">-&gt; Come posso liberare il nano?</span> <span class="yarn-meta">#line:how_free_dwarf </span>
<span class="yarn-line">    Trova la chiave. Poi apri la gabbia!</span> <span class="yarn-meta">#line:055d8b9 </span>
<span class="yarn-choice">-&gt; Dov'è la chiave?</span> <span class="yarn-meta">#line:where_key </span>
<span class="yarn-line">    Sposta la pianta per trovarla!</span> <span class="yarn-meta">#line:0a5ff98 </span>
<span class="yarn-choice">-&gt; Dov'è il nano?</span> <span class="yarn-meta">#line:where_dwarf </span>
    <span class="yarn-cmd">&lt;&lt;SetMapIcon dwarf_0 on&gt;&gt;</span>
<span class="yarn-line">    Osserva la stella sulla mappa.</span> <span class="yarn-meta">#line:02da44a </span>
<span class="yarn-choice">-&gt; Arrivederci</span> <span class="yarn-meta">#line:goodbye </span>

</code>
</pre>
</div>

<a id="ys-node-key-0"></a>

## key_0

<div class="yarn-node" data-title="key_0">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: school</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">L'hai trovato! Ora vai e libera il nano!</span> <span class="yarn-meta">#line:0289364 </span>
<span class="yarn-cmd">&lt;&lt;target dwarf_0&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;collect&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-dwarf-0"></a>

## dwarf_0

<div class="yarn-node" data-title="dwarf_0">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">group: school</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $dwarf_0_done&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card dwarf_expert&gt;&gt;</span>
<span class="yarn-line">    Molto tempo fa, ho costruito dei tunnel nanici in città.</span> <span class="yarn-meta">#line:0e45f38 </span>
    <span class="yarn-cmd">&lt;&lt;card tram&gt;&gt;</span>
<span class="yarn-line">    Ora si sono trasformati in veicoli magici chiamati "tram".</span> <span class="yarn-meta">#line:cf_014b</span>
<span class="yarn-line">    Ti regalo questo biglietto speciale per viaggiare ovunque!</span> <span class="yarn-meta">#line:0c46606 </span>
    <span class="yarn-cmd">&lt;&lt;card wroclaw_map&gt;&gt;</span>
<span class="yarn-line">    Ora vai e libera tutti e 10 i miei amici nani.</span> <span class="yarn-meta">#line:0b401f5 </span>
    <span class="yarn-cmd">&lt;&lt;camera_focus camera_cathedral_overview&gt;&gt;</span>
<span class="yarn-line">    Per prima cosa, recatevi alla Cattedrale.</span> <span class="yarn-meta">#line:0816b22 </span>
    <span class="yarn-cmd">&lt;&lt;camera_reset&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;target driver_tram_school&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif has_item("key_gold")&gt;&gt;</span>
<span class="yarn-line">    Per sbloccare la gabbia, risolvi questo enigma!</span> <span class="yarn-meta">#line:solve_to_unlock</span>
    <span class="yarn-cmd">&lt;&lt;activity activity_0 activity_0_done&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    È chiuso a chiave. Trova una chiave d'oro!</span> <span class="yarn-meta">#line:locked</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-activity-0-done"></a>

## activity_0_done

<div class="yarn-node" data-title="activity_0_done">
<pre class="yarn-code" style="--node-color:---"><code>
<span class="yarn-header-dim">group: school</span>
<span class="yarn-header-dim">color:</span>
<span class="yarn-header-dim">---</span>
&lt;&lt;if GetActivityResult("") &gt; 0&gt;&gt;
    <span class="yarn-cmd">&lt;&lt;task_end dwarf_0&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;inventory key_gold remove&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;inventory tram add&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;SetActive cage_0 false&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;SetMapIcon dwarf_0 done&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $dwarf_0_done = true&gt;&gt;</span>
<span class="yarn-line">    Grazie! Mi hai salvato!</span> <span class="yarn-meta">#line:dwarf_saved</span>
    <span class="yarn-cmd">&lt;&lt;jump dwarf_0&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Riprova più tardi!</span> <span class="yarn-meta">#line:try_again_later</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-chest-0"></a>

## chest_0

<div class="yarn-node" data-title="chest_0">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: school</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $dwarf_0_done&gt;&gt;</span>
<span class="yarn-line">    Il forziere si apre!</span> <span class="yarn-meta">#shadow:chest_opens</span>
    <span class="yarn-cmd">&lt;&lt;trigger chest_0_open&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;SetInteractable chest_0 false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Lo scrigno è chiuso a chiave.</span> <span class="yarn-meta">#shadow:chest_locked</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-intro-area-cathedral"></a>

## intro_area_cathedral

<div class="yarn-node" data-title="intro_area_cathedral">
<pre class="yarn-code" style="--node-color:blue"><code>
<span class="yarn-header-dim">// -----------------------------------------------------------------------------</span>
<span class="yarn-header-dim">// AREA CATHEDRAL</span>
<span class="yarn-header-dim">group: cathedral</span>
<span class="yarn-header-dim">type: panel</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;set $current_place = "cathedral"&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;area area_cathedral&gt;&gt;</span>
<span class="yarn-line">Ci troviamo alla fermata Bema, vicino alla cattedrale.</span> <span class="yarn-meta">#line:0d7ac4b </span>
<span class="yarn-cmd">&lt;&lt;if !$dwarf_1_done&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;target npc_1&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-npc-1"></a>

## npc_1

<div class="yarn-node" data-title="npc_1">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">group: cathedral</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $dwarf_1_done&gt;&gt;</span>
<span class="yarn-line">    Utilizza il tram per scegliere la tua prossima fermata.</span> <span class="yarn-meta">#line:03991b3 </span>
    <span class="yarn-cmd">&lt;&lt;target driver_tram_cathedral&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif GetCurrentTask() == "dwarf_1"&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;detour info_1&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card dwarf_bishop&gt;&gt;</span>
<span class="yarn-line">    Il vescovo nano è rinchiuso qui vicino.</span> <span class="yarn-meta">#line:0769945 </span>
<span class="yarn-line">    Trova la chiave della cattedrale e liberalo!</span> <span class="yarn-meta">#line:0ebb3bc </span>
<span class="yarn-line">    Parlami se hai bisogno di aiuto.</span> <span class="yarn-meta">#shadow:talk_4_help</span>
    <span class="yarn-cmd">&lt;&lt;task_start dwarf_1&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-info-1"></a>

## info_1

<div class="yarn-node" data-title="info_1">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">group: cathedral</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card wroclaw_cathedral&gt;&gt;</span>
<span class="yarn-line">Questa è la cattedrale di Breslavia.</span> <span class="yarn-meta">#line:0eb59b5</span>
<span class="yarn-line">La cattedrale si trova in una delle zone più antiche di Breslavia.</span> <span class="yarn-meta">#line:info_1a</span>
<span class="yarn-line">Le persone vengono qui per pregare e riflettere in tranquillità.</span> <span class="yarn-meta">#line:info_1b</span>
<span class="yarn-cmd">&lt;&lt;camera_focus camera_cathedral&gt;&gt;</span>
<span class="yarn-line">Ha alte torri e bellissime finestre.</span> <span class="yarn-meta">#line:07f68f0 </span>
<span class="yarn-cmd">&lt;&lt;camera_reset&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-hint-1"></a>

## hint_1

<div class="yarn-node" data-title="hint_1">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: cathedral</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Come posso essere d'aiuto?</span> <span class="yarn-meta">#shadow:how_can_help </span>
<span class="yarn-choice">-&gt; Come posso liberare il nano?</span> <span class="yarn-meta">#shadow:how_free_dwarf </span>
<span class="yarn-line">    Trova la chiave. Poi apri la gabbia!</span> <span class="yarn-meta">#shadow:055d8b9</span>
<span class="yarn-choice">-&gt; Dov'è la chiave?</span> <span class="yarn-meta">#shadow:where_key</span>
    <span class="yarn-cmd">&lt;&lt;target ll_key_1&gt;&gt;</span>
<span class="yarn-line">    Qualcuno qui vicino ce l'ha.</span> <span class="yarn-meta">#line:0166015</span>
<span class="yarn-choice">-&gt; Dov'è il nano?</span> <span class="yarn-meta">#shadow:where_dwarf </span>
    <span class="yarn-cmd">&lt;&lt;SetMapIcon dwarf_1 on&gt;&gt;</span>
<span class="yarn-line">    Osserva la stella sulla mappa.</span> <span class="yarn-meta">#shadow:02da44a</span>
<span class="yarn-choice">-&gt; Arrivederci</span> <span class="yarn-meta">#shadow:goodbye</span>

</code>
</pre>
</div>

<a id="ys-node-ll-key-1"></a>

## ll_key_1

<div class="yarn-node" data-title="ll_key_1">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: cathedral</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if GetCurrentTask() == "dwarf_1"&gt;&gt;</span>
<span class="yarn-line">Cosa fanno le persone in una cattedrale?</span> <span class="yarn-meta">#line:0e6869e </span>
<span class="yarn-choice">-&gt; Pregano</span> <span class="yarn-meta">#line:0a1aab0 </span>
<span class="yarn-line">    Esatto! Ecco la chiave.</span> <span class="yarn-meta">#line:correct_key </span>
    <span class="yarn-cmd">&lt;&lt;SetActive key_1 true&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;target key_1&gt;&gt;</span>
<span class="yarn-choice">-&gt; Essi leggono</span> <span class="yarn-meta">#line:0e25e89 </span>
<span class="yarn-line">    No. Riprova!</span> <span class="yarn-meta">#line:try_again</span>
<span class="yarn-choice">-&gt; Giocano</span> <span class="yarn-meta">#line:0340188 </span>
<span class="yarn-line">    No. Riprova!</span> <span class="yarn-meta">#shadow:try_again</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump info_1&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-key-1"></a>

## key_1

<div class="yarn-node" data-title="key_1">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: cathedral</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">L'hai trovato! Ora vai e libera il nano!</span> <span class="yarn-meta">#shadow:0289364 </span>
<span class="yarn-cmd">&lt;&lt;target dwarf_1&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;collect&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-dwarf-1"></a>

## dwarf_1

<div class="yarn-node" data-title="dwarf_1">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">group: cathedral</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $dwarf_1_done&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card dwarf_bishop&gt;&gt;</span>
<span class="yarn-line">    Rimango qui per aiutare le persone a trovare la pace.</span> <span class="yarn-meta">#line:08ae2a5 </span>
<span class="yarn-line">    I miei amici hanno ancora bisogno di aiuto al Centennial Hall e in centro città.</span> <span class="yarn-meta">#line:0b7a174 </span>
    <span class="yarn-cmd">&lt;&lt;target driver_tram_cathedral&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif has_item("key_gold")&gt;&gt;</span>
<span class="yarn-line">    Per sbloccare la gabbia, risolvi questo enigma!</span> <span class="yarn-meta">#shadow:solve_to_unlock</span>
    <span class="yarn-cmd">&lt;&lt;activity activity_1 activity_1_done&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    È chiuso a chiave. Trova una chiave d'oro!</span> <span class="yarn-meta">#shadow:locked</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-activity-1-done"></a>

## activity_1_done

<div class="yarn-node" data-title="activity_1_done">
<pre class="yarn-code" style="--node-color:---"><code>
<span class="yarn-header-dim">group: cathedral</span>
<span class="yarn-header-dim">color:</span>
<span class="yarn-header-dim">---</span>
&lt;&lt;if GetActivityResult("") &gt; 0&gt;&gt;
    <span class="yarn-cmd">&lt;&lt;task_end dwarf_1&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;inventory key_gold remove&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;inventory wroclaw_map add&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;inventory wroclaw_dwarf_statue add&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;SetActive cage_1 false&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;SetMapIcon dwarf_1 done&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $dwarf_1_done = true&gt;&gt;</span>
<span class="yarn-line">    Grazie! Mi hai salvato!</span> <span class="yarn-meta">#shadow:dwarf_saved</span>
    <span class="yarn-cmd">&lt;&lt;card wroclaw_map&gt;&gt;</span>
<span class="yarn-line">    Ho disegnato una mappa di Breslavia.</span> <span class="yarn-meta">#line:0fcb25a </span>
<span class="yarn-line">    Usalo per vedere dove ti trovi e dove devi andare!</span> <span class="yarn-meta">#line:0d493c7</span>
    <span class="yarn-cmd">&lt;&lt;jump dwarf_1&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Riprova più tardi!</span> <span class="yarn-meta">#shadow:try_again_later</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-chest-1"></a>

## chest_1

<div class="yarn-node" data-title="chest_1">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: cathedral</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $dwarf_1_done&gt;&gt;</span>
<span class="yarn-line">    Il forziere si apre!</span> <span class="yarn-meta">#shadow:chest_opens</span>
    <span class="yarn-cmd">&lt;&lt;trigger chest_1_open&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;SetInteractable chest_1 false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Lo scrigno è chiuso a chiave.</span> <span class="yarn-meta">#shadow:chest_locked</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-intro-area-center"></a>

## intro_area_center

<div class="yarn-node" data-title="intro_area_center">
<pre class="yarn-code" style="--node-color:blue"><code>
<span class="yarn-header-dim">// -----------------------------------------------------------------------------</span>
<span class="yarn-header-dim">// AREA CENTER</span>
<span class="yarn-header-dim">group: center</span>
<span class="yarn-header-dim">type: panel</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;area area_center&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;set $current_place = "center"&gt;&gt;</span>
<span class="yarn-line">Ammirate Piazza del Mercato, il Municipio e il Panorama Racławicka!</span> <span class="yarn-meta">#line:0ccc1a3 </span>
<span class="yarn-cmd">&lt;&lt;if !$dwarf_2_done&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;target npc_2&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-check-area-center"></a>

## check_area_center

<div class="yarn-node" data-title="check_area_center">
<pre class="yarn-code" style="--node-color:blue"><code>
<span class="yarn-header-dim">group: center</span>
<span class="yarn-header-dim">type:</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if !$dwarf_2_done&gt;&gt;</span>
<span class="yarn-line">    Il Nano Origine è disponibile al mercato.</span> <span class="yarn-meta">#line:08a9541 </span>
    <span class="yarn-cmd">&lt;&lt;target npc_2&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif !$dwarf_3_done&gt;&gt;</span>
<span class="yarn-line">    Il consigliere nano si trova vicino al vecchio municipio.</span> <span class="yarn-meta">#line:councilor_near_old_town_hall</span>
    <span class="yarn-cmd">&lt;&lt;target npc_3&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif !$dwarf_4_done&gt;&gt;</span>
<span class="yarn-line">    Panorama Racławicka è l'ultimo luogo di questa zona.</span> <span class="yarn-meta">#line:panorama_last_place_here</span>
    <span class="yarn-cmd">&lt;&lt;card tram&gt;&gt;</span>
<span class="yarn-line">    Se non vuoi camminare, puoi prendere il tram.</span> <span class="yarn-meta">#line:use_tram</span>
    <span class="yarn-cmd">&lt;&lt;target npc_4&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    La zona centrale è completata!</span> <span class="yarn-meta">#line:center_area_done</span>
    <span class="yarn-cmd">&lt;&lt;set $area_center_done = true&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;target driver_tram_center&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-npc-2"></a>

## npc_2

<div class="yarn-node" data-title="npc_2">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">group: center</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $dwarf_2_done&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump check_area_center&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif GetCurrentTask() == "dwarf_2"&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump hint_2&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;detour info_2&gt;&gt;</span>
<span class="yarn-line">    Breslavia è famosa per i suoi piccoli nani.</span> <span class="yarn-meta">#line:05b17f6 </span>
<span class="yarn-line">    Hanno cappellini e stivali minuscoli!</span> <span class="yarn-meta">#line:0f0ca08 </span>
<span class="yarn-line">    Liberate il Nano Originario.</span> <span class="yarn-meta">#line:04fde74 </span>
<span class="yarn-line">    Parlami se hai bisogno di aiuto.</span> <span class="yarn-meta">#shadow:talk_4_help</span>
    <span class="yarn-cmd">&lt;&lt;task_start dwarf_2&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;SetActive key_2 true&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-info-2"></a>

## info_2

<div class="yarn-node" data-title="info_2">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">group: center</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card wroclaw_market_square&gt;&gt;</span>
<span class="yarn-line">Piazza del Mercato è uno dei luoghi più frequentati di Breslavia.</span> <span class="yarn-meta">#line:info_2a</span>
<span class="yarn-cmd">&lt;&lt;card wroclaw_dwarf_statue&gt;&gt;</span>
<span class="yarn-line">Molte persone, mentre passeggiano, cercano statue di nani!</span> <span class="yarn-meta">#line:info_2b</span>

</code>
</pre>
</div>

<a id="ys-node-hint-2"></a>

## hint_2

<div class="yarn-node" data-title="hint_2">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: center</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Come posso essere d'aiuto?</span> <span class="yarn-meta">#shadow:how_can_help </span>
<span class="yarn-choice">-&gt; Come posso liberare il nano?</span> <span class="yarn-meta">#shadow:how_free_dwarf </span>
<span class="yarn-line">    Trova la chiave. Poi apri la gabbia!</span> <span class="yarn-meta">#shadow:055d8b9</span>
<span class="yarn-choice">-&gt; Dov'è la chiave?</span> <span class="yarn-meta">#shadow:where_key</span>
<span class="yarn-line">    Una chiave si trova sotto un vaso di fiori al mercato.</span> <span class="yarn-meta">#line:0d121cc</span>
    <span class="yarn-cmd">&lt;&lt;target key_2&gt;&gt;</span>
<span class="yarn-line">    Se non li trovi tutti, usa la mappa!</span> <span class="yarn-meta">#line:use_map_find</span>
<span class="yarn-choice">-&gt; Dov'è il nano?</span> <span class="yarn-meta">#shadow:where_dwarf </span>
    <span class="yarn-cmd">&lt;&lt;SetMapIcon dwarf_2 on&gt;&gt;</span>
<span class="yarn-line">    Osserva la stella sulla mappa.</span> <span class="yarn-meta">#shadow:02da44a</span>
<span class="yarn-choice">-&gt; Arrivederci</span> <span class="yarn-meta">#shadow:goodbye</span>

</code>
</pre>
</div>

<a id="ys-node-key-2"></a>

## key_2

<div class="yarn-node" data-title="key_2">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: center</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Ecco la tua chiave!</span> <span class="yarn-meta">#line:a_key</span>
<span class="yarn-cmd">&lt;&lt;collect&gt;&gt;</span>
<span class="yarn-line">Ora puoi sbloccare il nano!</span> <span class="yarn-meta">#shadow:unlock_dwarf</span>
<span class="yarn-cmd">&lt;&lt;target dwarf_2&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-dwarf-2"></a>

## dwarf_2

<div class="yarn-node" data-title="dwarf_2">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">group: center</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $dwarf_2_done&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card dwarf_origin&gt;&gt;</span>
<span class="yarn-line">    Sono uno dei nani più anziani della città! Sono apparso per la prima volta nel 2005.</span> <span class="yarn-meta">#line:025f12a</span>
<span class="yarn-line">    In città ci sono più di 800 nani!</span> <span class="yarn-meta">#line:08150b1 </span>
<span class="yarn-line">    Riuscirai a trovarci tutti?</span> <span class="yarn-meta">#line:09ff330 </span>
    <span class="yarn-cmd">&lt;&lt;target npc_2&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif has_item("key_gold")&gt;&gt;</span>
<span class="yarn-line">    Per sbloccare la gabbia, risolvi questo enigma!</span> <span class="yarn-meta">#shadow:solve_to_unlock</span>
    <span class="yarn-cmd">&lt;&lt;activity activity_2 activity_2_done&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    È chiuso a chiave. Trova una chiave d'oro!</span> <span class="yarn-meta">#shadow:locked</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-activity-2-done"></a>

## activity_2_done

<div class="yarn-node" data-title="activity_2_done">
<pre class="yarn-code" style="--node-color:---"><code>
<span class="yarn-header-dim">group: center</span>
<span class="yarn-header-dim">color:</span>
<span class="yarn-header-dim">---</span>
&lt;&lt;if GetActivityResult("") &gt; 0&gt;&gt;
    <span class="yarn-cmd">&lt;&lt;task_end dwarf_2&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;inventory key_gold remove&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;inventory wroclaw_dwarf_statue add&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;SetActive cage_2 false&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;SetMapIcon dwarf_2 done&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $dwarf_2_done = true&gt;&gt;</span>
<span class="yarn-line">    Grazie! Mi hai salvato!</span> <span class="yarn-meta">#shadow:dwarf_saved</span>
    <span class="yarn-cmd">&lt;&lt;jump dwarf_2&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Riprova più tardi!</span> <span class="yarn-meta">#shadow:try_again_later</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-chest-2"></a>

## chest_2

<div class="yarn-node" data-title="chest_2">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: center</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $dwarf_2_done&gt;&gt;</span>
<span class="yarn-comment">	// &lt;&lt;inventory key_gold remove&gt;&gt;</span>
<span class="yarn-line">	Il forziere si apre!</span> <span class="yarn-meta">#line:chest_opens</span>
    <span class="yarn-cmd">&lt;&lt;trigger chest_2_open&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;SetInteractable chest_2 false&gt;&gt;</span>
<span class="yarn-comment">	// &lt;&lt;SetActive dwarf_2 true&gt;&gt;</span>
<span class="yarn-comment">	// &lt;&lt;SetMapIcon chest_2 off&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Lo scrigno è chiuso a chiave.</span> <span class="yarn-meta">#line:chest_locked</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-npc-3"></a>

## npc_3

<div class="yarn-node" data-title="npc_3">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">group: center</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $dwarf_3_done&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump check_area_center&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif GetCurrentTask() == "dwarf_3"&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump hint_3&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif HasCompletedTask("dwarf_3") &gt;&gt;</span>
<span class="yarn-line">    Vai dal nano con la chiave!</span> <span class="yarn-meta">#line:go_dwarf_key</span>
    <span class="yarn-cmd">&lt;&lt;target dwarf_3&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;detour info_3&gt;&gt;</span>
<span class="yarn-line">    Qui, nel vecchio municipio, custodiamo le leggi della città.</span> <span class="yarn-meta">#line:0de0a2f </span>
<span class="yarn-line">    Ma il nostro consigliere nano ha perso tutti i suoi libri!</span> <span class="yarn-meta">#line:04f7a17 </span>
<span class="yarn-line">    Trovate tutti i libri sui nani!</span> <span class="yarn-meta">#line:02ba9a9 </span>
<span class="yarn-line">    Parlami se hai bisogno di aiuto.</span> <span class="yarn-meta">#shadow:talk_4_help</span>
    <span class="yarn-cmd">&lt;&lt;camera_map_distance 30&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;task_start dwarf_3 task_3_done&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-task-3-done"></a>

## task_3_done

<div class="yarn-node" data-title="task_3_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: panorama</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Li hai trovati tutti! Torna indietro per ricevere una ricompensa.</span> <span class="yarn-meta">#line:collected_all</span>
<span class="yarn-cmd">&lt;&lt;SetActive key_3 true&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;target key_3&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-info-3"></a>

## info_3

<div class="yarn-node" data-title="info_3">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">group: center</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card town_hall&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;card wroclaw_old_town_hall&gt;&gt;</span>
<span class="yarn-line">Il vecchio municipio è un edificio antico e importante.</span> <span class="yarn-meta">#line:info_3a</span>
<span class="yarn-line">Fu costruito intorno all'anno 1300!</span> <span class="yarn-meta">#line:info_3b</span>

</code>
</pre>
</div>

<a id="ys-node-hint-3"></a>

## hint_3

<div class="yarn-node" data-title="hint_3">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: center</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Come posso essere d'aiuto?</span> <span class="yarn-meta">#shadow:how_can_help </span>
<span class="yarn-choice">-&gt; Come posso liberare il nano?</span> <span class="yarn-meta">#shadow:how_free_dwarf </span>
<span class="yarn-line">    Trova la chiave. Poi apri la gabbia!</span> <span class="yarn-meta">#shadow:055d8b9</span>
<span class="yarn-choice">-&gt; Dov'è la chiave?</span> <span class="yarn-meta">#shadow:where_key</span>
<span class="yarn-line">    Raccogli tutti i libri.</span> <span class="yarn-meta">#line:0d450fd </span>
<span class="yarn-line">    Se non li trovi tutti, usa la mappa!</span> <span class="yarn-meta">#shadow:use_map_find</span>
<span class="yarn-choice">-&gt; Dov'è il nano?</span> <span class="yarn-meta">#shadow:where_dwarf </span>
    <span class="yarn-cmd">&lt;&lt;SetMapIcon dwarf_3 on&gt;&gt;</span>
<span class="yarn-line">    Osserva la stella sulla mappa.</span> <span class="yarn-meta">#shadow:02da44a</span>
<span class="yarn-choice">-&gt; Arrivederci</span> <span class="yarn-meta">#shadow:goodbye</span>

</code>
</pre>
</div>

<a id="ys-node-key-3"></a>

## key_3

<div class="yarn-node" data-title="key_3">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: center</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Ecco la tua chiave!</span> <span class="yarn-meta">#shadow:a_key</span>
<span class="yarn-cmd">&lt;&lt;collect&gt;&gt;</span>
<span class="yarn-line">Ora puoi sbloccare il nano!</span> <span class="yarn-meta">#line:unlock_dwarf</span>
<span class="yarn-cmd">&lt;&lt;target dwarf_3&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-dwarf-3"></a>

## dwarf_3

<div class="yarn-node" data-title="dwarf_3">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">group: center</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $dwarf_3_done&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card dwarf_councilor&gt;&gt;</span>
<span class="yarn-line">    Io sono il Consigliere Nano. Aiuto il Sindaco della Città!</span> <span class="yarn-meta">#line:0851c6f </span>
<span class="yarn-line">    La mia famiglia vive qui da tantissimo tempo!</span> <span class="yarn-meta">#line:06002b8 </span>
    <span class="yarn-cmd">&lt;&lt;target npc_4&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif has_item("key_gold")&gt;&gt;</span>
<span class="yarn-line">    Per sbloccare la gabbia, risolvi questo enigma!</span> <span class="yarn-meta">#shadow:solve_to_unlock</span>
    <span class="yarn-cmd">&lt;&lt;activity activity_3 activity_3_done&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    È chiuso a chiave. Trova una chiave d'oro!</span> <span class="yarn-meta">#shadow:locked</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-activity-3-done"></a>

## activity_3_done

<div class="yarn-node" data-title="activity_3_done">
<pre class="yarn-code" style="--node-color:---"><code>
<span class="yarn-header-dim">group: center</span>
<span class="yarn-header-dim">color:</span>
<span class="yarn-header-dim">---</span>
&lt;&lt;if GetActivityResult("") &gt; 0&gt;&gt;
    <span class="yarn-cmd">&lt;&lt;task_end dwarf_3&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;inventory key_gold remove&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;inventory wroclaw_dwarf_statue add&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;SetActive cage_3 false&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;SetMapIcon dwarf_3 done&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $dwarf_3_done = true&gt;&gt;</span>
<span class="yarn-line">    Grazie! Mi hai salvato!</span> <span class="yarn-meta">#shadow:dwarf_saved</span>
    <span class="yarn-cmd">&lt;&lt;jump dwarf_3&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Riprova più tardi!</span> <span class="yarn-meta">#shadow:try_again_later</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-chest-3"></a>

## chest_3

<div class="yarn-node" data-title="chest_3">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: center</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $dwarf_3_done&gt;&gt;</span>
<span class="yarn-line">    Il forziere si apre!</span> <span class="yarn-meta">#shadow:chest_opens</span>
    <span class="yarn-cmd">&lt;&lt;trigger chest_3_open&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;SetInteractable chest_3 false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Lo scrigno è chiuso a chiave.</span> <span class="yarn-meta">#shadow:chest_locked</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-intro-area-panorama"></a>

## intro_area_panorama

<div class="yarn-node" data-title="intro_area_panorama">
<pre class="yarn-code" style="--node-color:blue"><code>
<span class="yarn-header-dim">group: panorama</span>
<span class="yarn-header-dim">type: panel</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;area area_panorama&gt;&gt;</span>
<span class="yarn-line">Panorama Racławicka è un dipinto gigante e rotondo!</span> <span class="yarn-meta">#line:0fb27ac </span>
<span class="yarn-line">È un'immagine che ti circonda completamente.</span> <span class="yarn-meta">#line:016bb86 </span>
<span class="yarn-line">Trova qui il nano pittore.</span> <span class="yarn-meta">#line:0c878db </span>

</code>
</pre>
</div>

<a id="ys-node-npc-4"></a>

## npc_4

<div class="yarn-node" data-title="npc_4">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">group: panorama</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $dwarf_4_done&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump check_area_center&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif GetCurrentTask() == "dwarf_4"&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump hint_4&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif HasCompletedTask("dwarf_4") &gt;&gt;</span>
<span class="yarn-line">    Vai dal nano con la chiave!</span> <span class="yarn-meta">#shadow:go_dwarf_key</span>
    <span class="yarn-cmd">&lt;&lt;target dwarf_4&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;detour info_4&gt;&gt;</span>
<span class="yarn-line">    Il nostro nano pittore ha perso tutti i suoi colori!</span> <span class="yarn-meta">#line:07177c7 </span>
<span class="yarn-line">    Trova tutti i colori!</span> <span class="yarn-meta">#line:0737a2f</span>
<span class="yarn-line">    Parlami se hai bisogno di aiuto.</span> <span class="yarn-meta">#shadow:talk_4_help</span>
    <span class="yarn-cmd">&lt;&lt;camera_map_distance 30&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;task_start dwarf_4 task_4_done&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-task-4-done"></a>

## task_4_done

<div class="yarn-node" data-title="task_4_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: panorama</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Li hai trovati tutti! Torna indietro per ricevere una ricompensa.</span> <span class="yarn-meta">#shadow:collected_all</span>
<span class="yarn-cmd">&lt;&lt;SetActive key_4 true&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;target key_4&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-info-4"></a>

## info_4

<div class="yarn-node" data-title="info_4">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">group: panorama</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card panorama_racawicka&gt;&gt;</span>
<span class="yarn-line">Panorama Racławicka è un'enorme immagine che avvolge l'intera stanza!</span> <span class="yarn-meta">#line:info_4a</span>
<span class="yarn-line">Ti fa sentire come se fossi dentro il dipinto!</span> <span class="yarn-meta">#line:info_4b</span>

</code>
</pre>
</div>

<a id="ys-node-hint-4"></a>

## hint_4

<div class="yarn-node" data-title="hint_4">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: panorama</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Come posso essere d'aiuto?</span> <span class="yarn-meta">#shadow:how_can_help </span>
<span class="yarn-choice">-&gt; Come posso liberare il nano?</span> <span class="yarn-meta">#shadow:how_free_dwarf </span>
<span class="yarn-line">    Trova la chiave. Poi apri la gabbia!</span> <span class="yarn-meta">#shadow:055d8b9</span>
<span class="yarn-choice">-&gt; Dov'è la chiave?</span> <span class="yarn-meta">#shadow:where_key</span>
<span class="yarn-line">    Raccogli tutti i colori presenti nel parco.</span> <span class="yarn-meta">#line:key_under_the_frame</span>
<span class="yarn-line">    Se non li trovi tutti, usa la mappa!</span> <span class="yarn-meta">#shadow:use_map_find</span>
<span class="yarn-choice">-&gt; Dov'è il nano?</span> <span class="yarn-meta">#shadow:where_dwarf </span>
    <span class="yarn-cmd">&lt;&lt;SetMapIcon dwarf_4 on&gt;&gt;</span>
<span class="yarn-line">    Osserva la stella sulla mappa.</span> <span class="yarn-meta">#shadow:02da44a</span>
<span class="yarn-choice">-&gt; Arrivederci</span> <span class="yarn-meta">#shadow:goodbye</span>

</code>
</pre>
</div>

<a id="ys-node-key-4"></a>

## key_4

<div class="yarn-node" data-title="key_4">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: panorama</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Ecco la tua chiave!</span> <span class="yarn-meta">#shadow:a_key</span>
<span class="yarn-cmd">&lt;&lt;collect&gt;&gt;</span>
<span class="yarn-line">Ora puoi sbloccare il nano!</span> <span class="yarn-meta">#shadow:unlock_dwarf</span>
<span class="yarn-cmd">&lt;&lt;target dwarf_4&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-dwarf-4"></a>

## dwarf_4

<div class="yarn-node" data-title="dwarf_4">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">group: panorama</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $dwarf_4_done&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card dwarf_painter&gt;&gt;</span>
<span class="yarn-line">    Dipingo la storia di Breslavia affinché tutti possano vederla.</span> <span class="yarn-meta">#line:dwarf4_003</span>
<span class="yarn-line">    Adoro i colori vivaci e le immagini giganti!</span> <span class="yarn-meta">#line:dwarf4_004</span>
<span class="yarn-cmd">&lt;&lt;elseif has_item("key_gold")&gt;&gt;</span>
<span class="yarn-line">    Per sbloccare la gabbia, risolvi questo enigma!</span> <span class="yarn-meta">#shadow:solve_to_unlock</span>
    <span class="yarn-cmd">&lt;&lt;activity activity_4 activity_4_done&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    È chiuso a chiave. Trova una chiave d'oro!</span> <span class="yarn-meta">#shadow:locked</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-activity-4-done"></a>

## activity_4_done

<div class="yarn-node" data-title="activity_4_done">
<pre class="yarn-code" style="--node-color:---"><code>
<span class="yarn-header-dim">group: panorama</span>
<span class="yarn-header-dim">color:</span>
<span class="yarn-header-dim">---</span>
&lt;&lt;if GetActivityResult("") &gt; 0&gt;&gt;
    <span class="yarn-cmd">&lt;&lt;task_end dwarf_4&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;inventory key_gold remove&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;inventory wroclaw_dwarf_statue add&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;SetActive cage_4 false&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;SetMapIcon dwarf_4 done&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $dwarf_4_done = true&gt;&gt;</span>
<span class="yarn-line">    Grazie! Mi hai salvato!</span> <span class="yarn-meta">#shadow:dwarf_saved</span>
    <span class="yarn-cmd">&lt;&lt;jump dwarf_4&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Riprova più tardi!</span> <span class="yarn-meta">#shadow:try_again_later</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-chest-4"></a>

## chest_4

<div class="yarn-node" data-title="chest_4">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: panorama</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $dwarf_4_done&gt;&gt;</span>
<span class="yarn-line">    Il forziere si apre!</span> <span class="yarn-meta">#shadow:chest_opens</span>
    <span class="yarn-cmd">&lt;&lt;trigger chest_4_open&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;SetInteractable chest_4 false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Lo scrigno è chiuso a chiave.</span> <span class="yarn-meta">#shadow:chest_locked</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-intro-area-centennial"></a>

## intro_area_centennial

<div class="yarn-node" data-title="intro_area_centennial">
<pre class="yarn-code" style="--node-color:blue"><code>
<span class="yarn-header-dim">// -----------------------------------------------------------------------------</span>
<span class="yarn-header-dim">// AREA CENTENNIAL HALL</span>
<span class="yarn-header-dim">group: centennial</span>
<span class="yarn-header-dim">type: panel</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;set $current_place = "centennial"&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;area area_centennial&gt;&gt;</span>
<span class="yarn-line">Benvenuti alla Centennial Hall!</span> <span class="yarn-meta">#line:0db2417 </span>
<span class="yarn-line">Dai un'occhiata allo zoo, alla grande sala e alla fontana.</span> <span class="yarn-meta">#line:05bd1c1 </span>
<span class="yarn-cmd">&lt;&lt;if !$dwarf_5_done&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;target npc_5&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-check-area-centennial"></a>

## check_area_centennial

<div class="yarn-node" data-title="check_area_centennial">
<pre class="yarn-code" style="--node-color:blue"><code>
<span class="yarn-header-dim">group: centennial</span>
<span class="yarn-header-dim">type:</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if !$dwarf_5_done&gt;&gt;</span>
<span class="yarn-line">    Allo zoo c'è sempre qualcosa da fare!</span> <span class="yarn-meta">#line:0b13c52 </span>
    <span class="yarn-cmd">&lt;&lt;target npc_5&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif !$dwarf_6_done&gt;&gt;</span>
<span class="yarn-line">    Hai controllato all'interno della Centennial Hall?</span> <span class="yarn-meta">#line:095f631 </span>
    <span class="yarn-cmd">&lt;&lt;target npc_6&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif !$dwarf_7_done&gt;&gt;</span>
<span class="yarn-line">    La Fontana Multimediale ha bisogno del nostro aiuto!</span> <span class="yarn-meta">#line:0bc156b </span>
    <span class="yarn-cmd">&lt;&lt;target npc_7&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $area_centennial_done = true&gt;&gt;</span>
<span class="yarn-line">    L'area della Centennial Hall è completata!</span> <span class="yarn-meta">#line:0d209ba </span>
    <span class="yarn-cmd">&lt;&lt;target driver_tram_centennial&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-npc-5"></a>

## npc_5

<div class="yarn-node" data-title="npc_5">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">// ZOO</span>
<span class="yarn-header-dim">group: centennial</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $dwarf_5_done&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump check_area_centennial&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif GetCurrentTask() == "dwarf_5"&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump hint_5&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif HasCompletedTask("dwarf_5") &gt;&gt;</span>
<span class="yarn-line">    Vai dal nano con la chiave!</span> <span class="yarn-meta">#shadow:go_dwarf_key</span>
    <span class="yarn-cmd">&lt;&lt;target dwarf_5&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;detour info_5&gt;&gt;</span>
<span class="yarn-line">    Il nostro nano amante degli animali adora tutti gli animali</span> <span class="yarn-meta">#line:048c7ab </span>
<span class="yarn-line">    Ma sono scappati via quando è passato un grosso cane blu!</span> <span class="yarn-meta">#line:05deab1 </span>
<span class="yarn-line">    Trova tutti gli animali e libera il nano!</span> <span class="yarn-meta">#line:09b7b40 </span>
    <span class="yarn-cmd">&lt;&lt;camera_map_distance 30&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;task_start dwarf_5 task_5_done&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-task-5-done"></a>

## task_5_done

<div class="yarn-node" data-title="task_5_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: centennial</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Li hai trovati tutti! Torna indietro per ricevere una ricompensa.</span> <span class="yarn-meta">#shadow:collected_all</span>
<span class="yarn-cmd">&lt;&lt;SetActive key_5 true&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;target key_5&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-info-5"></a>

## info_5

<div class="yarn-node" data-title="info_5">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">group: centennial</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card wroclaw_dwarfs&gt;&gt;</span>
<span class="yarn-line">Lo zoo di Breslavia è il più grande della Polonia!</span> <span class="yarn-meta">#line:07c9455</span>
<span class="yarn-line">Lo zoo è stato costruito molto tempo fa, nel 1865!</span> <span class="yarn-meta">#line:info_5a</span>
<span class="yarn-line">Le famiglie vengono qui per vedere animali provenienti da tutto il mondo.</span> <span class="yarn-meta">#line:info_5b</span>

</code>
</pre>
</div>

<a id="ys-node-ll-spawn-5"></a>

## ll_spawn_5

<div class="yarn-node" data-title="ll_spawn_5">
<pre class="yarn-code" style="--node-color:---"><code>
<span class="yarn-header-dim">group: centennial</span>
<span class="yarn-header-dim">color:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Mi piace lo zoo!</span> <span class="yarn-meta">#line:08a038b </span>
<span class="yarn-line">Voglio proteggere tutti gli animali</span> <span class="yarn-meta">#line:09033f1 </span>
<span class="yarn-line">Dov'è il mio gatto?</span> <span class="yarn-meta">#line:01dc29f </span>

</code>
</pre>
</div>

<a id="ys-node-hint-5"></a>

## hint_5

<div class="yarn-node" data-title="hint_5">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: centennial</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Come posso essere d'aiuto?</span> <span class="yarn-meta">#shadow:how_can_help </span>
<span class="yarn-choice">-&gt; Come posso liberare il nano?</span> <span class="yarn-meta">#shadow:how_free_dwarf </span>
<span class="yarn-line">    Trova la chiave. Poi apri la gabbia!</span> <span class="yarn-meta">#shadow:055d8b9</span>
<span class="yarn-choice">-&gt; Dov'è la chiave?</span> <span class="yarn-meta">#shadow:where_key</span>
<span class="yarn-line">    Trova tutti gli animali presenti nello zoo.</span> <span class="yarn-meta">#line:0d0946a </span>
<span class="yarn-line">    Se non li trovi tutti, usa la mappa!</span> <span class="yarn-meta">#shadow:use_map_find</span>
<span class="yarn-choice">-&gt; Dov'è il nano?</span> <span class="yarn-meta">#shadow:where_dwarf </span>
    <span class="yarn-cmd">&lt;&lt;SetMapIcon dwarf_5 on&gt;&gt;</span>
<span class="yarn-line">    Osserva la stella sulla mappa.</span> <span class="yarn-meta">#shadow:02da44a</span>
<span class="yarn-choice">-&gt; Arrivederci</span> <span class="yarn-meta">#shadow:goodbye</span>

</code>
</pre>
</div>

<a id="ys-node-key-5"></a>

## key_5

<div class="yarn-node" data-title="key_5">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: centennial</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Ecco la tua chiave!</span> <span class="yarn-meta">#shadow:a_key</span>
<span class="yarn-cmd">&lt;&lt;collect&gt;&gt;</span>
<span class="yarn-line">Ora puoi sbloccare il nano!</span> <span class="yarn-meta">#shadow:unlock_dwarf</span>
<span class="yarn-cmd">&lt;&lt;target dwarf_5&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-dwarf-5"></a>

## dwarf_5

<div class="yarn-node" data-title="dwarf_5">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">group: centennial</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $dwarf_5_done&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card dwarf_animal_lover&gt;&gt;</span>
<span class="yarn-line">    Mi prendo cura degli animali. Anche tu ami gli animali?</span> <span class="yarn-meta">#line:dwarf5_001</span>
<span class="yarn-line">    Spero che ti divertirai allo zoo!</span> <span class="yarn-meta">#line:dwarf5_002</span>
    <span class="yarn-cmd">&lt;&lt;jump check_area_centennial&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif has_item("key_gold")&gt;&gt;</span>
<span class="yarn-line">    Per sbloccare la gabbia, risolvi questo enigma!</span> <span class="yarn-meta">#shadow:solve_to_unlock</span>
    <span class="yarn-cmd">&lt;&lt;activity activity_5 activity_5_done&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    È chiuso a chiave. Trova una chiave d'oro!</span> <span class="yarn-meta">#shadow:locked</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-activity-5-done"></a>

## activity_5_done

<div class="yarn-node" data-title="activity_5_done">
<pre class="yarn-code" style="--node-color:---"><code>
<span class="yarn-header-dim">group: centennial</span>
<span class="yarn-header-dim">color:</span>
<span class="yarn-header-dim">---</span>
&lt;&lt;if GetActivityResult("") &gt; 0&gt;&gt;
    <span class="yarn-cmd">&lt;&lt;task_end dwarf_5&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;inventory key_gold remove&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;inventory wroclaw_dwarf_statue add&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;SetActive cage_5 false&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;SetMapIcon dwarf_5 done&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $dwarf_5_done = true&gt;&gt;</span>
<span class="yarn-line">    Grazie! Mi hai salvato!</span> <span class="yarn-meta">#shadow:dwarf_saved</span>
    <span class="yarn-cmd">&lt;&lt;jump dwarf_5&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Riprova più tardi!</span> <span class="yarn-meta">#shadow:try_again_later</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-chest-5"></a>

## chest_5

<div class="yarn-node" data-title="chest_5">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: centennial</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $dwarf_5_done&gt;&gt;</span>
<span class="yarn-line">    Il forziere si apre!</span> <span class="yarn-meta">#shadow:chest_opens</span>
    <span class="yarn-cmd">&lt;&lt;trigger chest_5_open&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;SetInteractable chest_5 false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Lo scrigno è chiuso a chiave.</span> <span class="yarn-meta">#shadow:chest_locked</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-npc-6"></a>

## npc_6

<div class="yarn-node" data-title="npc_6">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">// CENTENNIAL HALL</span>
<span class="yarn-header-dim">group: centennial</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $dwarf_6_done&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump check_area_centennial&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif GetCurrentTask() == "dwarf_6"&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump hint_6&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;detour info_6&gt;&gt;</span>
<span class="yarn-line">    Aiuto! Il nostro nano architetto è scomparso!</span> <span class="yarn-meta">#line:0c3a081 </span>
<span class="yarn-line">    Abbiamo bisogno di lui per migliorare gli edifici</span> <span class="yarn-meta">#line:088d0bd </span>
<span class="yarn-line">    Trova e salva il nano architetto!</span> <span class="yarn-meta">#line:092e472</span>
<span class="yarn-line">    Parlami se hai bisogno di aiuto.</span> <span class="yarn-meta">#shadow:talk_4_help</span>
    <span class="yarn-cmd">&lt;&lt;task_start dwarf_6&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;SetActive key_6 true&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-info-6"></a>

## info_6

<div class="yarn-node" data-title="info_6">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">group: centennial</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card wroclaw_centennial_hall&gt;&gt;</span>
<span class="yarn-line">La Centennial Hall ha un enorme tetto rotondo.</span> <span class="yarn-meta">#line:info_6a</span>
<span class="yarn-line">È uno degli edifici più famosi della città!</span> <span class="yarn-meta">#line:info_6b</span>

</code>
</pre>
</div>

<a id="ys-node-hint-6"></a>

## hint_6

<div class="yarn-node" data-title="hint_6">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: centennial</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Come posso essere d'aiuto?</span> <span class="yarn-meta">#shadow:how_can_help </span>
<span class="yarn-choice">-&gt; Come posso liberare il nano?</span> <span class="yarn-meta">#shadow:how_free_dwarf </span>
<span class="yarn-line">    Trova la chiave. Poi apri la gabbia!</span> <span class="yarn-meta">#shadow:055d8b9</span>
<span class="yarn-choice">-&gt; Dov'è la chiave?</span> <span class="yarn-meta">#shadow:where_key</span>
    <span class="yarn-cmd">&lt;&lt;camera_focus camera_centennial_iglica&gt;&gt;</span>
<span class="yarn-line">    Lo vedo! È in cima all'Iglica!</span> <span class="yarn-meta">#line:0abe6d9</span>
    <span class="yarn-cmd">&lt;&lt;camera_reset&gt;&gt;</span>
<span class="yarn-choice">-&gt; Dov'è il nano?</span> <span class="yarn-meta">#shadow:where_dwarf</span>
    <span class="yarn-cmd">&lt;&lt;camera_focus camera_centennial&gt;&gt;</span>
<span class="yarn-line">    Qualcuno lo ha visto salire sul tetto</span> <span class="yarn-meta">#line:0a7bbd6 </span>
    <span class="yarn-cmd">&lt;&lt;camera_reset&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;SetMapIcon dwarf_6 on&gt;&gt;</span>
<span class="yarn-choice">-&gt; Arrivederci</span> <span class="yarn-meta">#shadow:goodbye</span>

</code>
</pre>
</div>

<a id="ys-node-lever-6"></a>

## lever_6

<div class="yarn-node" data-title="lever_6">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: centennial</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;camera_focus camera_centennial_iglica&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;wait 3&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;camera_reset&gt;&gt;</span>
<span class="yarn-line">Ecco la tua chiave!</span> <span class="yarn-meta">#shadow:a_key</span>

</code>
</pre>
</div>

<a id="ys-node-key-6"></a>

## key_6

<div class="yarn-node" data-title="key_6">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: centennial</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;collect&gt;&gt;</span>
<span class="yarn-line">Ora puoi sbloccare il nano!</span> <span class="yarn-meta">#shadow:unlock_dwarf</span>
<span class="yarn-cmd">&lt;&lt;target dwarf_6&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-dwarf-6"></a>

## dwarf_6

<div class="yarn-node" data-title="dwarf_6">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">group: centennial</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $dwarf_6_done&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card dwarf_architect&gt;&gt;</span>
<span class="yarn-line">    Adoro i tetti e gli archi alti!</span> <span class="yarn-meta">#line:dwarf6_001</span>
<span class="yarn-line">    Penso sempre a nuovi edifici da disegnare.</span> <span class="yarn-meta">#line:dwarf6_002</span>
    <span class="yarn-cmd">&lt;&lt;jump check_area_centennial&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif has_item("key_gold")&gt;&gt;</span>
<span class="yarn-line">    Per sbloccare la gabbia, risolvi questo enigma!</span> <span class="yarn-meta">#shadow:solve_to_unlock</span>
    <span class="yarn-cmd">&lt;&lt;activity activity_6 activity_6_done&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    È chiuso a chiave. Trova una chiave d'oro!</span> <span class="yarn-meta">#shadow:locked</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-activity-6-done"></a>

## activity_6_done

<div class="yarn-node" data-title="activity_6_done">
<pre class="yarn-code" style="--node-color:---"><code>
<span class="yarn-header-dim">group: centennial</span>
<span class="yarn-header-dim">color:</span>
<span class="yarn-header-dim">---</span>
&lt;&lt;if GetActivityResult("") &gt; 0&gt;&gt;
    <span class="yarn-cmd">&lt;&lt;task_end dwarf_6&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;inventory key_gold remove&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;inventory wroclaw_dwarf_statue add&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;SetActive cage_6 false&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;SetMapIcon dwarf_6 done&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $dwarf_6_done = true&gt;&gt;</span>
<span class="yarn-line">    Grazie! Mi hai salvato!</span> <span class="yarn-meta">#shadow:dwarf_saved</span>
    <span class="yarn-cmd">&lt;&lt;jump dwarf_6&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Riprova più tardi!</span> <span class="yarn-meta">#shadow:try_again_later</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-chest-6"></a>

## chest_6

<div class="yarn-node" data-title="chest_6">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: centennial</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $dwarf_6_done&gt;&gt;</span>
<span class="yarn-line">    Il forziere si apre!</span> <span class="yarn-meta">#shadow:chest_opens</span>
    <span class="yarn-cmd">&lt;&lt;trigger chest_6_open&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;SetInteractable chest_6 false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Lo scrigno è chiuso a chiave.</span> <span class="yarn-meta">#shadow:chest_locked</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-npc-7"></a>

## npc_7

<div class="yarn-node" data-title="npc_7">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">// MULTIMEDIA FOUNTAIN</span>
<span class="yarn-header-dim">group: centennial</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $dwarf_7_done&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump check_area_centennial&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif GetCurrentTask() == "dwarf_7"&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump hint_7&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif HasCompletedTask("dwarf_7") &gt;&gt;</span>
<span class="yarn-line">    Vai dal nano con la chiave!</span> <span class="yarn-meta">#shadow:go_dwarf_key</span>
    <span class="yarn-cmd">&lt;&lt;target dwarf_7&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;detour info_7&gt;&gt;</span>
<span class="yarn-line">    Il nostro nano era solito dirigere lo spettacolo delle fontane.</span> <span class="yarn-meta">#line:01d8fa1 </span>
<span class="yarn-line">    Ma ora ha perso tutti gli appunti!</span> <span class="yarn-meta">#line:0f05296 </span>
<span class="yarn-line">    Trova la musica e salva il nano direttore d'orchestra!</span> <span class="yarn-meta">#line:041a696 </span>
<span class="yarn-line">    Parlami se hai bisogno di aiuto.</span> <span class="yarn-meta">#shadow:talk_4_help</span>
    <span class="yarn-cmd">&lt;&lt;camera_map_distance 30&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;task_start dwarf_7 task_7_done&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-task-7-done"></a>

## task_7_done

<div class="yarn-node" data-title="task_7_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: centennial</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Li hai trovati tutti! Torna indietro per ricevere una ricompensa.</span> <span class="yarn-meta">#shadow:collected_all</span>
<span class="yarn-cmd">&lt;&lt;SetActive key_7 true&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;target key_7&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-info-7"></a>

## info_7

<div class="yarn-node" data-title="info_7">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">group: centennial</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card wroclaw_multimedia_fountain&gt;&gt;</span>
<span class="yarn-line">Questa fontana utilizza acqua, luci colorate e musica!</span> <span class="yarn-meta">#line:info_7a </span>
<span class="yarn-line">È uno spettacolo "multimediale" perché utilizza molti elementi diversi.</span> <span class="yarn-meta">#line:info_7b</span>
<span class="yarn-line">Venite qui di notte per assistere allo spettacolo!</span> <span class="yarn-meta">#line:info_7c</span>

</code>
</pre>
</div>

<a id="ys-node-hint-7"></a>

## hint_7

<div class="yarn-node" data-title="hint_7">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: centennial</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Come posso essere d'aiuto?</span> <span class="yarn-meta">#shadow:how_can_help </span>
<span class="yarn-choice">-&gt; Come posso liberare il nano?</span> <span class="yarn-meta">#shadow:how_free_dwarf</span>
    <span class="yarn-cmd">&lt;&lt;card musical_scale&gt;&gt;</span>
<span class="yarn-line">    Suona le note nell'ordine corretto.</span> <span class="yarn-meta">#line:0ddb6b5 </span>
<span class="yarn-choice">-&gt; Dov'è la chiave?</span> <span class="yarn-meta">#shadow:where_key</span>
<span class="yarn-line">    Trova tutte le note musicali qui intorno</span> <span class="yarn-meta">#line:0d919ae </span>
<span class="yarn-line">    Se non li trovi tutti, usa la mappa!</span> <span class="yarn-meta">#shadow:use_map_find</span>
<span class="yarn-choice">-&gt; Dov'è il nano?</span> <span class="yarn-meta">#shadow:where_dwarf </span>
    <span class="yarn-cmd">&lt;&lt;SetMapIcon dwarf_7 on&gt;&gt;</span>
<span class="yarn-line">    Osserva la stella sulla mappa.</span> <span class="yarn-meta">#shadow:02da44a</span>
<span class="yarn-choice">-&gt; Arrivederci</span> <span class="yarn-meta">#shadow:goodbye</span>

</code>
</pre>
</div>

<a id="ys-node-key-7"></a>

## key_7

<div class="yarn-node" data-title="key_7">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: centennial</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Ecco la tua chiave!</span> <span class="yarn-meta">#shadow:a_key</span>
<span class="yarn-cmd">&lt;&lt;collect&gt;&gt;</span>
<span class="yarn-line">Ora puoi sbloccare il nano!</span> <span class="yarn-meta">#shadow:unlock_dwarf</span>
<span class="yarn-cmd">&lt;&lt;target dwarf_7&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-dwarf-7"></a>

## dwarf_7

<div class="yarn-node" data-title="dwarf_7">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">group: centennial</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $dwarf_7_done&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card dwarf_conductor&gt;&gt;</span>
<span class="yarn-line">    Lo spettacolo delle fontane sembra un grande concerto!</span> <span class="yarn-meta">#line:dwarf7_002</span>
    <span class="yarn-cmd">&lt;&lt;card piano&gt;&gt;</span>
<span class="yarn-line">    Suoni uno strumento musicale?</span> <span class="yarn-meta">#line:dwarf7_001</span>
    <span class="yarn-cmd">&lt;&lt;jump check_area_centennial&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif has_item("key_gold")&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card musical_scale&gt;&gt;</span>
<span class="yarn-line">    Suona le note per sbloccarmi!</span> <span class="yarn-meta">#line:0622e7b </span>
    <span class="yarn-cmd">&lt;&lt;activity activity_7 activity_7_done&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    È chiuso a chiave. Trova una chiave d'oro!</span> <span class="yarn-meta">#shadow:locked</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-activity-7-done"></a>

## activity_7_done

<div class="yarn-node" data-title="activity_7_done">
<pre class="yarn-code" style="--node-color:---"><code>
<span class="yarn-header-dim">group: centennial</span>
<span class="yarn-header-dim">color:</span>
<span class="yarn-header-dim">---</span>
&lt;&lt;if GetActivityResult("") &gt; 0&gt;&gt;
    <span class="yarn-cmd">&lt;&lt;task_end dwarf_7&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;inventory key_gold remove&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;inventory wroclaw_dwarf_statue add&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;SetActive cage_7 false&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;SetMapIcon dwarf_7 done&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $dwarf_7_done = true&gt;&gt;</span>
<span class="yarn-line">    Grazie! Mi hai salvato!</span> <span class="yarn-meta">#shadow:dwarf_saved</span>
    <span class="yarn-cmd">&lt;&lt;jump dwarf_7&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Riprova più tardi!</span> <span class="yarn-meta">#shadow:try_again_later</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-chest-7"></a>

## chest_7

<div class="yarn-node" data-title="chest_7">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: centennial</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $dwarf_7_done&gt;&gt;</span>
<span class="yarn-line">    Il forziere si apre!</span> <span class="yarn-meta">#shadow:chest_opens</span>
    <span class="yarn-cmd">&lt;&lt;trigger chest_7_open&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;SetInteractable chest_7 false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Lo scrigno è chiuso a chiave.</span> <span class="yarn-meta">#shadow:chest_locked</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-intro-area-skytower"></a>

## intro_area_skytower

<div class="yarn-node" data-title="intro_area_skytower">
<pre class="yarn-code" style="--node-color:blue"><code>
<span class="yarn-header-dim">// -----------------------------------------------------------------------------</span>
<span class="yarn-header-dim">// AREA SKY TOWER</span>
<span class="yarn-header-dim">group: skytower</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;set $current_place = "skytower"&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;area area_skytower&gt;&gt;</span>
<span class="yarn-line">Benvenuti alla Sky Tower! È altissima.</span> <span class="yarn-meta">#line:05da79c </span>

</code>
</pre>
</div>

<a id="ys-node-check-area-skytower"></a>

## check_area_skytower

<div class="yarn-node" data-title="check_area_skytower">
<pre class="yarn-code" style="--node-color:blue"><code>
<span class="yarn-header-dim">group: skytower</span>
<span class="yarn-header-dim">type:</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if !$dwarf_8_done&gt;&gt;</span>
<span class="yarn-line">    Il Nano Scrittore è ancora bloccato!</span> <span class="yarn-meta">#line:0eb3a6c </span>
    <span class="yarn-cmd">&lt;&lt;target npc_8&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif !$dwarf_9_done&gt;&gt;</span>
<span class="yarn-line">    Il nano viaggiatore si trova nella piazza.</span> <span class="yarn-meta">#line:06183de </span>
    <span class="yarn-cmd">&lt;&lt;target npc_9&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif !$dwarf_10_done&gt;&gt;</span>
<span class="yarn-line">    Il Custode delle Chiavi è intrappolato vicino all'ascensore!</span> <span class="yarn-meta">#line:0977e6d </span>
    <span class="yarn-cmd">&lt;&lt;target npc_10&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Tutti i nani della Torre Celeste sono gratis!</span> <span class="yarn-meta">#line:08a24f3 </span>
<span class="yarn-line">    L'ascensore è ora aperto.</span> <span class="yarn-meta">#line:0c540ac </span>
    <span class="yarn-cmd">&lt;&lt;target ll_elevator&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-npc-8"></a>

## npc_8

<div class="yarn-node" data-title="npc_8">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">// WRITER</span>
<span class="yarn-header-dim">group: skytower</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $dwarf_8_done&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump check_area_skytower&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif GetCurrentTask() == "dwarf_8"&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump hint_8&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;detour info_8&gt;&gt;</span>
<span class="yarn-line">    Aiuta il nano scrittore.</span> <span class="yarn-meta">#line:0ac8824 </span>
<span class="yarn-line">    Ama le storie, i libri e... le biblioteche.</span> <span class="yarn-meta">#line:082386e </span>
<span class="yarn-line">    Parlami se hai bisogno di aiuto.</span> <span class="yarn-meta">#shadow:talk_4_help</span>
    <span class="yarn-cmd">&lt;&lt;task_start dwarf_8&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;SetActive key_8 true&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-info-8"></a>

## info_8

<div class="yarn-node" data-title="info_8">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">group: skytower</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card olga_tokarczuk&gt;&gt;</span>
<span class="yarn-line">La famosa scrittrice Olga Tokarczuk viveva qui vicino!</span> <span class="yarn-meta">#line:0474fb4 </span>
<span class="yarn-line">Lei ha vinto un grande premio per i suoi libri: il Premio Nobel!</span> <span class="yarn-meta">#line:info_8a</span>
<span class="yarn-line">Magari potresti leggere un libro in biblioteca?</span> <span class="yarn-meta">#line:info_8b</span>

</code>
</pre>
</div>

<a id="ys-node-hint-8"></a>

## hint_8

<div class="yarn-node" data-title="hint_8">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: skytower</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">La chiave è vicino all'angolo lettura!</span> <span class="yarn-meta">#line:0d82e66 </span>
<span class="yarn-cmd">&lt;&lt;target key_8&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-key-8"></a>

## key_8

<div class="yarn-node" data-title="key_8">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: skytower</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Ecco la tua chiave!</span> <span class="yarn-meta">#shadow:a_key</span>
<span class="yarn-cmd">&lt;&lt;collect&gt;&gt;</span>
<span class="yarn-line">Ora puoi sbloccare il nano!</span> <span class="yarn-meta">#shadow:unlock_dwarf</span>
<span class="yarn-cmd">&lt;&lt;target dwarf_8&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-dwarf-8"></a>

## dwarf_8

<div class="yarn-node" data-title="dwarf_8">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">group: skytower</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $dwarf_8_done&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card dwarf_writer&gt;&gt;</span>
<span class="yarn-line">    Adoro avere un buon libro tra le mani!</span> <span class="yarn-meta">#line:dwarf8_001</span>
<span class="yarn-line">    A volte mi siedo qui e immagino le mie storie.</span> <span class="yarn-meta">#line:dwarf8_003</span>
    <span class="yarn-cmd">&lt;&lt;jump check_area_skytower&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif has_item("key_gold")&gt;&gt;</span>
<span class="yarn-line">    Per sbloccare la gabbia, risolvi questo enigma!</span> <span class="yarn-meta">#shadow:solve_to_unlock</span>
    <span class="yarn-cmd">&lt;&lt;activity activity_8 activity_8_done&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    È chiuso a chiave. Trova una chiave d'oro!</span> <span class="yarn-meta">#shadow:locked</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-activity-8-done"></a>

## activity_8_done

<div class="yarn-node" data-title="activity_8_done">
<pre class="yarn-code" style="--node-color:---"><code>
<span class="yarn-header-dim">group: skytower</span>
<span class="yarn-header-dim">color:</span>
<span class="yarn-header-dim">---</span>
&lt;&lt;if GetActivityResult("") &gt; 0&gt;&gt;
    <span class="yarn-cmd">&lt;&lt;task_end dwarf_8&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;inventory key_gold remove&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;inventory wroclaw_dwarf_statue add&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;SetActive cage_8 false&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;SetMapIcon dwarf_8 done&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $dwarf_8_done = true&gt;&gt;</span>
<span class="yarn-line">    Grazie! Mi hai salvato!</span> <span class="yarn-meta">#shadow:dwarf_saved</span>
    <span class="yarn-cmd">&lt;&lt;jump dwarf_8&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Riprova più tardi!</span> <span class="yarn-meta">#shadow:try_again_later</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-chest-8"></a>

## chest_8

<div class="yarn-node" data-title="chest_8">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: skytower</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $dwarf_8_done&gt;&gt;</span>
<span class="yarn-line">    Il forziere si apre!</span> <span class="yarn-meta">#shadow:chest_opens</span>
    <span class="yarn-cmd">&lt;&lt;trigger chest_8_open&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;SetInteractable chest_8 false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Lo scrigno è chiuso a chiave.</span> <span class="yarn-meta">#shadow:chest_locked</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-npc-9"></a>

## npc_9

<div class="yarn-node" data-title="npc_9">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">// TRAVELER</span>
<span class="yarn-header-dim">group: skytower</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $dwarf_9_done&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump check_area_skytower&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif GetCurrentTask() == "dwarf_9"&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump hint_9&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;detour info_9&gt;&gt;</span>
<span class="yarn-line">    Il nostro nano viaggiatore adora visitare posti nuovi.</span> <span class="yarn-meta">#line:info_017</span>
<span class="yarn-line">    Ma si è perso in un labirinto.</span> <span class="yarn-meta">#line:02172f2</span>
<span class="yarn-line">    Trova e salva il nano viaggiatore!</span> <span class="yarn-meta">#line:07cb84f </span>
<span class="yarn-line">    Parlami se hai bisogno di aiuto.</span> <span class="yarn-meta">#shadow:talk_4_help</span>
    <span class="yarn-cmd">&lt;&lt;camera_map_distance 30&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;task_start dwarf_9&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;SetActive key_9 true&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-info-9"></a>

## info_9

<div class="yarn-node" data-title="info_9">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">group: skytower</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card sky_tower_plaza&gt;&gt;</span>
<span class="yarn-line">Una piazza è un grande spazio aperto dove le persone si incontrano.</span> <span class="yarn-meta">#line:info_9a </span>
<span class="yarn-cmd">&lt;&lt;card dali_profile_of_time&gt;&gt;</span>
<span class="yarn-line">Qui si trova una famosa scultura dell'artista Salvador Dalí!</span> <span class="yarn-meta">#line:info_9c</span>

</code>
</pre>
</div>

<a id="ys-node-hint-9"></a>

## hint_9

<div class="yarn-node" data-title="hint_9">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: skytower</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Come posso essere d'aiuto?</span> <span class="yarn-meta">#shadow:how_can_help </span>
<span class="yarn-choice">-&gt; Come posso liberare il nano?</span> <span class="yarn-meta">#shadow:how_free_dwarf</span>
<span class="yarn-line">   Prendi la chiave che si trova sopra i libri.</span> <span class="yarn-meta">#line:07430fb </span>
<span class="yarn-choice">-&gt; Dov'è la chiave?</span> <span class="yarn-meta">#shadow:where_key</span>
    <span class="yarn-cmd">&lt;&lt;target key_9&gt;&gt;</span>
<span class="yarn-line">    Muoviti e salta sopra gli scaffali per prenderlo.</span> <span class="yarn-meta">#line:0acc3e1</span>
<span class="yarn-choice">-&gt; Dov'è il nano?</span> <span class="yarn-meta">#shadow:where_dwarf </span>
    <span class="yarn-cmd">&lt;&lt;SetMapIcon dwarf_9 on&gt;&gt;</span>
<span class="yarn-line">    Osserva la stella sulla mappa.</span> <span class="yarn-meta">#shadow:02da44a</span>
<span class="yarn-choice">-&gt; Arrivederci</span> <span class="yarn-meta">#shadow:goodbye</span>

</code>
</pre>
</div>

<a id="ys-node-key-9"></a>

## key_9

<div class="yarn-node" data-title="key_9">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: skytower</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Ecco la tua chiave!</span> <span class="yarn-meta">#shadow:a_key</span>
<span class="yarn-cmd">&lt;&lt;collect&gt;&gt;</span>
<span class="yarn-line">Ora puoi sbloccare il nano!</span> <span class="yarn-meta">#shadow:unlock_dwarf</span>
<span class="yarn-cmd">&lt;&lt;target dwarf_9&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-dwarf-9"></a>

## dwarf_9

<div class="yarn-node" data-title="dwarf_9">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">group: skytower</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $dwarf_9_done&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card dwarf_traveler&gt;&gt;</span>
<span class="yarn-line">    Adoro ammirare quella scultura di Salvador Dalí!</span> <span class="yarn-meta">#line:dwarf9_001</span>
<span class="yarn-line">    Un giorno vorrei viaggiare in tutto il mondo.</span> <span class="yarn-meta">#line:dwarf9_002</span>
    <span class="yarn-cmd">&lt;&lt;jump check_area_skytower&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif has_item("key_gold")&gt;&gt;</span>
<span class="yarn-line">    Per sbloccare la gabbia, risolvi questo enigma!</span> <span class="yarn-meta">#shadow:solve_to_unlock</span>
    <span class="yarn-cmd">&lt;&lt;activity activity_9 activity_9_done&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    È chiuso a chiave. Trova una chiave d'oro!</span> <span class="yarn-meta">#shadow:locked</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-activity-9-done"></a>

## activity_9_done

<div class="yarn-node" data-title="activity_9_done">
<pre class="yarn-code" style="--node-color:---"><code>
<span class="yarn-header-dim">group: skytower</span>
<span class="yarn-header-dim">color:</span>
<span class="yarn-header-dim">---</span>
&lt;&lt;if GetActivityResult("") &gt; 0&gt;&gt;
    <span class="yarn-cmd">&lt;&lt;task_end dwarf_9&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;inventory key_gold remove&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;inventory wroclaw_dwarf_statue add&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;SetActive cage_9 false&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;SetMapIcon dwarf_9 done&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;camera_map_distance -1&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $dwarf_9_done = true&gt;&gt;</span>
<span class="yarn-line">    Grazie! Mi hai salvato!</span> <span class="yarn-meta">#shadow:dwarf_saved</span>
    <span class="yarn-cmd">&lt;&lt;jump dwarf_9&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Riprova più tardi!</span> <span class="yarn-meta">#shadow:try_again_later</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-chest-9"></a>

## chest_9

<div class="yarn-node" data-title="chest_9">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: skytower</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $dwarf_9_done&gt;&gt;</span>
<span class="yarn-line">    Il forziere si apre!</span> <span class="yarn-meta">#shadow:chest_opens</span>
    <span class="yarn-cmd">&lt;&lt;trigger chest_9_open&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;SetInteractable chest_9 false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Lo scrigno è chiuso a chiave.</span> <span class="yarn-meta">#shadow:chest_locked</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-npc-10"></a>

## npc_10

<div class="yarn-node" data-title="npc_10">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">// KEYMASTER</span>
<span class="yarn-header-dim">group: skytower</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $dwarf_10_done&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump check_area_skytower&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif GetCurrentTask() == "dwarf_10"&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump hint_10&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;if $dwarf_8_done and $dwarf_9_done&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;detour info_10&gt;&gt;</span>
<span class="yarn-line">        Il round finale!</span> <span class="yarn-meta">#line:0e6a977 </span>
<span class="yarn-line">        Il Custode delle Chiavi ha bisogno delle 10 Chiavi dei Nani!</span> <span class="yarn-meta">#line:0e72fd8</span>
<span class="yarn-line">        Parlami se hai bisogno di aiuto.</span> <span class="yarn-meta">#shadow:talk_4_help</span>
        <span class="yarn-cmd">&lt;&lt;camera_map_distance 30&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;task_start dwarf_10 task_10_done&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">        Sblocca prima gli altri due nani, poi torna qui.</span> <span class="yarn-meta">#line:0a8844b </span>
        <span class="yarn-cmd">&lt;&lt;if !$dwarf_8_done&gt;&gt;</span>
            <span class="yarn-cmd">&lt;&lt;target npc_8&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
            <span class="yarn-cmd">&lt;&lt;target npc_9&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-task-10-done"></a>

## task_10_done

<div class="yarn-node" data-title="task_10_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: centennial</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Vai dal Maestro dei Nani!</span> <span class="yarn-meta">#line:06bc72f </span>
<span class="yarn-cmd">&lt;&lt;target dwarf_10&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-info-10"></a>

## info_10

<div class="yarn-node" data-title="info_10">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">group: skytower</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card wroclaw_sky_tower&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;camera_focus camera_skytower_roof&gt;&gt;</span>
<span class="yarn-line">Sky Tower è uno degli edifici più alti di Breslavia.</span> <span class="yarn-meta">#line:info_10a</span>
<span class="yarn-line">Dall'alto si può vedere quasi tutta la città!</span> <span class="yarn-meta">#line:info_10b</span>
<span class="yarn-cmd">&lt;&lt;camera_reset&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-hint-10"></a>

## hint_10

<div class="yarn-node" data-title="hint_10">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: skytower</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">A questo nano manca solo un'ultima chiave per essere libero!</span> <span class="yarn-meta">#line:031d093 </span>
<span class="yarn-cmd">&lt;&lt;SetActive key_10 true&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;target key_10&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-key-10"></a>

## key_10

<div class="yarn-node" data-title="key_10">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: skytower</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Ecco la tua chiave!</span> <span class="yarn-meta">#shadow:a_key</span>
<span class="yarn-cmd">&lt;&lt;collect&gt;&gt;</span>
<span class="yarn-line">Ora puoi sbloccare il nano!</span> <span class="yarn-meta">#shadow:unlock_dwarf</span>
<span class="yarn-cmd">&lt;&lt;target dwarf_10&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-dwarf-10"></a>

## dwarf_10

<div class="yarn-node" data-title="dwarf_10">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">group: skytower</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $dwarf_10_done&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card dwarf_keymaster&gt;&gt;</span>
<span class="yarn-line">    Finalmente posso di nuovo sorvegliare tutte le serrature!</span> <span class="yarn-meta">#line:dwarf10_001</span>
<span class="yarn-line">    Ti aprirò l'ascensore.</span> <span class="yarn-meta">#line:0c6f241</span>
    <span class="yarn-cmd">&lt;&lt;jump check_area_skytower&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif HasCompletedTask("dwarf_10") &gt;&gt;</span>
<span class="yarn-line">    Per sbloccare la gabbia, risolvi questo enigma!</span> <span class="yarn-meta">#shadow:solve_to_unlock</span>
    <span class="yarn-cmd">&lt;&lt;activity activity_10 activity_10_done&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    È chiuso a chiave. Trova una chiave d'oro!</span> <span class="yarn-meta">#shadow:locked</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-activity-10-done"></a>

## activity_10_done

<div class="yarn-node" data-title="activity_10_done">
<pre class="yarn-code" style="--node-color:---"><code>
<span class="yarn-header-dim">group: skytower</span>
<span class="yarn-header-dim">color:</span>
<span class="yarn-header-dim">---</span>
&lt;&lt;if GetActivityResult("") &gt; 0&gt;&gt;
    <span class="yarn-cmd">&lt;&lt;inventory key_gold remove&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;inventory wroclaw_dwarf_statue add&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;SetActive cage_10 false&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;SetMapIcon dwarf_10 done&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $dwarf_10_done = true&gt;&gt;</span>
<span class="yarn-line">    Grazie! Mi hai salvato!</span> <span class="yarn-meta">#shadow:dwarf_saved</span>
    <span class="yarn-cmd">&lt;&lt;jump dwarf_10&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Riprova più tardi!</span> <span class="yarn-meta">#shadow:try_again_later</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-chest-10"></a>

## chest_10

<div class="yarn-node" data-title="chest_10">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: skytower</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $dwarf_10_done&gt;&gt;</span>
<span class="yarn-line">    Il forziere si apre!</span> <span class="yarn-meta">#shadow:chest_opens</span>
    <span class="yarn-cmd">&lt;&lt;trigger chest_10_open&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;SetInteractable chest_10 false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Lo scrigno è chiuso a chiave.</span> <span class="yarn-meta">#shadow:chest_locked</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-ll-elevator"></a>

## ll_elevator

<div class="yarn-node" data-title="ll_elevator">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">// -----------------------------------------------------------------------------</span>
<span class="yarn-header-dim">// SKY TOWER TOP AND FINALE</span>
<span class="yarn-header-dim">group: finale</span>
<span class="yarn-header-dim">actor: GUIDE_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $dwarf_10_done&gt;&gt;</span>
<span class="yarn-line">Vuoi salire sul tetto?</span> <span class="yarn-meta">#line:088ccf1 </span>
<span class="yarn-choice">-&gt; SÌ</span> <span class="yarn-meta">#line:yes</span>
<span class="yarn-line">    Saliamo in cima alla Sky Tower!</span> <span class="yarn-meta">#line:0bca140</span>
    <span class="yarn-cmd">&lt;&lt;SetInteractable elevatore_level true&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;trigger elevator&gt;&gt;</span>
<span class="yarn-comment">    // &lt;&lt;action activate_elevator&gt;&gt;</span>
<span class="yarn-choice">-&gt; NO</span> <span class="yarn-meta">#line:no </span>
<span class="yarn-line">    Torna quando sei pronto.</span> <span class="yarn-meta">#line:0b51799</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">L'ascensore non funziona!</span> <span class="yarn-meta">#line:0a94351 </span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-npc-roof"></a>

## npc_roof

<div class="yarn-node" data-title="npc_roof">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: finale</span>
<span class="yarn-header-dim">actor: GUIDE_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">La città è bellissima vista da quassù!</span> <span class="yarn-meta">#line:035a519</span>
<span class="yarn-line">E guarda... anche Antura è qui!</span> <span class="yarn-meta">#line:05950e6 </span>
<span class="yarn-line">Vuoi rispondere ad alcune domande per completare questa missione?</span> <span class="yarn-meta">#line:071d502 </span>
<span class="yarn-choice">-&gt; SÌ</span> <span class="yarn-meta">#shadow:yes</span>
    <span class="yarn-cmd">&lt;&lt;jump assessment_q1&gt;&gt;</span>
<span class="yarn-choice">-&gt; No, voglio restare qui</span> <span class="yarn-meta">#line:021c568 </span>
<span class="yarn-line">    Torna quando sei pronto.</span> <span class="yarn-meta">#shadow:0b51799 </span>

</code>
</pre>
</div>

<a id="ys-node-assessment-q1"></a>

## assessment_q1

<div class="yarn-node" data-title="assessment_q1">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: finale</span>
<span class="yarn-header-dim">actor: GUIDE_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Qual è un famoso simbolo di Breslavia?</span> <span class="yarn-meta">#line:0509f87 </span>
<span class="yarn-choice">-&gt; Una fontana</span> <span class="yarn-meta">#line:0ad5b67 </span>
<span class="yarn-line">    No. Riprova!</span> <span class="yarn-meta">#shadow:try_again</span>
    <span class="yarn-cmd">&lt;&lt;jump assessment_q1&gt;&gt;</span>
<span class="yarn-choice">-&gt; Un nano</span> <span class="yarn-meta">#line:0b73e95 </span>
<span class="yarn-line">    Sì, è proprio così!</span> <span class="yarn-meta">#line:yes_right </span>
    <span class="yarn-cmd">&lt;&lt;jump assessment_q2&gt;&gt;</span>
<span class="yarn-choice">-&gt; Una chiave</span> <span class="yarn-meta">#line:0c5d850 </span>
<span class="yarn-line">    No. Riprova!</span> <span class="yarn-meta">#shadow:try_again</span>
    <span class="yarn-cmd">&lt;&lt;jump assessment_q1&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-assessment-q2"></a>

## assessment_q2

<div class="yarn-node" data-title="assessment_q2">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: finale</span>
<span class="yarn-header-dim">actor: GUIDE_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Cosa fanno le persone in una cattedrale?</span> <span class="yarn-meta">#line:07cc31d </span>
<span class="yarn-choice">-&gt; Pregano</span> <span class="yarn-meta">#line:0416812 </span>
<span class="yarn-line">    Sì, è proprio così!</span> <span class="yarn-meta">#shadow:yes_right </span>
    <span class="yarn-cmd">&lt;&lt;jump assessment_q3&gt;&gt;</span>
<span class="yarn-choice">-&gt; Essi leggono</span> <span class="yarn-meta">#line:0bab60b </span>
<span class="yarn-line">    No. Riprova!</span> <span class="yarn-meta">#shadow:try_again</span>
    <span class="yarn-cmd">&lt;&lt;jump assessment_q2&gt;&gt;</span>
<span class="yarn-choice">-&gt; Giocano</span> <span class="yarn-meta">#line:0509c68 </span>
<span class="yarn-line">    No. Riprova!</span> <span class="yarn-meta">#shadow:try_again</span>
    <span class="yarn-cmd">&lt;&lt;jump assessment_q2&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-assessment-q3"></a>

## assessment_q3

<div class="yarn-node" data-title="assessment_q3">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: finale</span>
<span class="yarn-header-dim">actor: GUIDE_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Chi è Olga Tokarczuk?</span> <span class="yarn-meta">#line:0041cc7 </span>
<span class="yarn-choice">-&gt; Un politico</span> <span class="yarn-meta">#line:0aa7047 </span>
<span class="yarn-line">    No. Riprova!</span> <span class="yarn-meta">#shadow:try_again</span>
    <span class="yarn-cmd">&lt;&lt;jump assessment_q3&gt;&gt;</span>
<span class="yarn-choice">-&gt; Uno scienziato</span> <span class="yarn-meta">#line:0c4a0b5 </span>
<span class="yarn-line">    No. Riprova!</span> <span class="yarn-meta">#shadow:try_again</span>
    <span class="yarn-cmd">&lt;&lt;jump assessment_q3&gt;&gt;</span>
<span class="yarn-choice">-&gt; Uno scrittore</span> <span class="yarn-meta">#line:09368b4 </span>
<span class="yarn-line">    Sì, è proprio così!</span> <span class="yarn-meta">#shadow:yes_right </span>
    <span class="yarn-cmd">&lt;&lt;jump assessment_end&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-assessment-end"></a>

## assessment_end

<div class="yarn-node" data-title="assessment_end">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: finale</span>
<span class="yarn-header-dim">actor: GUIDE_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Ottimo lavoro! Hai esplorato Breslavia e liberato tutti i nani.</span> <span class="yarn-meta">#line:0cedfa4 </span>
<span class="yarn-cmd">&lt;&lt;jump quest_end&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-arcade"></a>

## arcade

<div class="yarn-node" data-title="arcade">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">group: activities</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Vuoi fare una partita?</span> <span class="yarn-meta">#line:08e8a18 </span>
<span class="yarn-choice">-&gt; Memoria</span> <span class="yarn-meta">#line:game_memory </span>
    <span class="yarn-cmd">&lt;&lt;activity activity_9 arcade normal&gt;&gt;</span>
<span class="yarn-choice">-&gt; Puzzle</span> <span class="yarn-meta">#line:game_jigsaw</span>
    <span class="yarn-cmd">&lt;&lt;activity activity_6 arcade normal&gt;&gt;</span>
<span class="yarn-choice">-&gt; NO</span> <span class="yarn-meta">#shadow:no</span>

</code>
</pre>
</div>

<a id="ys-node-spawned-tourist"></a>

## spawned_tourist

<div class="yarn-node" data-title="spawned_tourist">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">///////// NPCs SPAWNED IN THE SCENE //////////</span>
<span class="yarn-header-dim">// these npc are spawn automatically in the scene</span>
<span class="yarn-header-dim">// use these to add random facts. everythime you meet them</span>
<span class="yarn-header-dim">// they will say one of these lines randomly</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: ADULT_F</span>
<span class="yarn-header-dim">spawn_group: tourists</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Il panorama sembra un'immagine gigantesca che mi circonda!</span> <span class="yarn-meta">#line:055554f </span>
<span class="yarn-line">Molte persone visitano Breslavia alla ricerca di statue di nani.</span> <span class="yarn-meta">#line:0d17afb </span>
<span class="yarn-line">La Centennial Hall è famosa per il suo grande tetto rotondo.</span> <span class="yarn-meta">#line:005b996 </span>
<span class="yarn-line">La fontana multimediale utilizza acqua, luce e musica.</span> <span class="yarn-meta">#line:07d11df </span>
<span class="yarn-line">Sky Tower è uno degli edifici più alti di Breslavia.</span> <span class="yarn-meta">#shadow:info_10a </span>
<span class="yarn-line">Piazza del Mercato è un luogo molto frequentato e divertente.</span> <span class="yarn-meta">#line:0173c2b </span>
<span class="yarn-line">Alcuni nani di Breslavia hanno un aspetto buffo, altri invece sembrano seri!</span> <span class="yarn-meta">#line:0805d72 </span>

</code>
</pre>
</div>


