---
title: Mont Blanc i góry (fr_08) - Script
hide:
---

# Mont Blanc i góry (fr_08) - Script
> [!note] Educators & Designers: help improving this quest!
> **Comments and feedback**: [discuss in the Forum](https://antura.discourse.group/t/fr-08-mont-blanc-mountains/27/1)  
> **Improve script translations**: [comment the Google Sheet](https://docs.google.com/spreadsheets/d/1FPFOy8CHor5ArSg57xMuPAG7WM27-ecDOiU-OmtHgjw/edit?gid=736863861#gid=736863861)  
> **Improve Cards translations**: [comment the Google Sheet](https://docs.google.com/spreadsheets/d/1M3uOeqkbE4uyDs5us5vO-nAFT8Aq0LGBxjjT_CSScWw/edit?gid=415931977#gid=415931977)  
> **Improve the script**: [propose an edit here](https://github.com/vgwb/Antura/blob/main/Assets/_discover/_quests/FR_08%20Mont%20Blanc/FR_08%20Mont%20Blanc%20-%20Yarn%20Script.yarn)  

<a id="ys-node-quest-start"></a>

## quest_start

<div class="yarn-node" data-title="quest_start">
<pre class="yarn-code" style="--node-color:red"><code>
<span class="yarn-header-dim">// fr_08 | Mont Blanc</span>
<span class="yarn-header-dim">// </span>
<span class="yarn-header-dim">type: panel</span>
<span class="yarn-header-dim">color: red</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;declare $step_base1 = 0&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $step_base2 = 0&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $step_base3 = 0&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $step_base4 = 0&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;SetActive key_gold_intro false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;SetActive key_gold_1 false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;SetActive key_gold_2 false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;SetActive key_gold_3 false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;SetActive backpack false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;SetActive scarf false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;SetActive hat false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;SetActive sunglasses false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;SetActive crampons false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;card alps&gt;&gt;</span>
<span class="yarn-line">Witamy w Alpach Francuskich!</span> <span class="yarn-meta">#line:start_1</span>
<span class="yarn-cmd">&lt;&lt;card place_mont_blanc&gt;&gt;</span>
<span class="yarn-line">Dzisiaj wspinamy się na Mont Blanc. To najwyższy szczyt Alp.</span> <span class="yarn-meta">#line:start_2</span>
<span class="yarn-cmd">&lt;&lt;target npc_intro&gt;&gt;</span>

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
<span class="yarn-cmd">&lt;&lt;card place_mont_blanc&gt;&gt;</span>
<span class="yarn-line">Niesamowite! Dotarłeś na szczyt Mont Blanc. Ma 4807 metrów wysokości!</span> <span class="yarn-meta">#line:end_1</span>
<span class="yarn-line">Alpy są pełne wysokich, pięknych gór.</span> <span class="yarn-meta">#line:end_2</span>
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
<span class="yarn-line">Narysuj Mont Blanc i trzy flagi w swoim zeszycie!</span> <span class="yarn-meta">#line:post_1</span>
<span class="yarn-cmd">&lt;&lt;quest_end&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-npc-intro"></a>

## npc_intro

<div class="yarn-node" data-title="npc_intro">
<pre class="yarn-code" style="--node-color:blue"><code>
<span class="yarn-header-dim">// ---------------------------------------</span>
<span class="yarn-header-dim">// INTRO</span>
<span class="yarn-header-dim">// ---------------------------------------</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">group: intro</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if has_item("backpack")&gt;&gt;</span>
<span class="yarn-line">    Widzę, że masz plecak. Możesz iść!</span> <span class="yarn-meta">#line:04c1c1f </span>
    <span class="yarn-cmd">&lt;&lt;card place_mont_blanc&gt;&gt;</span>
<span class="yarn-line">    Wejdźmy na Mont Blanc.</span> <span class="yarn-meta">#line:0f4644b </span>
    <span class="yarn-cmd">&lt;&lt;card climbing&gt;&gt;</span>
<span class="yarn-line">    Znajdź cały sprzęt wspinaczkowy i dowiedz się więcej o trzech pobliskich krajach!</span> <span class="yarn-meta">#line:07f2699 </span>
    <span class="yarn-cmd">&lt;&lt;SetActive door_intro false&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;target npc_base_1&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif GetCurrentTask() == "get_backpack"&gt;&gt;</span>
<span class="yarn-line">    Wróć, gdy skończysz zadanie.</span> <span class="yarn-meta">#line:046eb33 </span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card mountain_guide&gt;&gt;</span>
<span class="yarn-line">    Ratunku! Jestem przewodnikiem górskim. Niebieski pies zabrał nasz sprzęt wspinaczkowy!</span> <span class="yarn-meta">#line:0724de3 </span>
<span class="yarn-line">    Widzieliśmy psa wbiegającego na górę.</span> <span class="yarn-meta">#line:01beb5e </span>
    <span class="yarn-cmd">&lt;&lt;camera_focus camera_mont_blanc&gt;&gt;</span>
<span class="yarn-line">    Zapraszamy do eksploracji Mont Blanc i zapoznania się z naszym sprzętem.</span> <span class="yarn-meta">#line:0650acc </span>
    <span class="yarn-cmd">&lt;&lt;card backpack&gt;&gt;</span>
<span class="yarn-line">    Przed wyjazdem będziesz potrzebować plecaka.</span> <span class="yarn-meta">#line:045409f</span>
    <span class="yarn-cmd">&lt;&lt;camera_focus camera_intro&gt;&gt;</span>
<span class="yarn-line">    Rozejrzyj się dookoła!</span> <span class="yarn-meta">#line:05cabb5 </span>
    <span class="yarn-cmd">&lt;&lt;task_start get_backpack&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;SetActive key_gold_intro true&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-chest-intro"></a>

## chest_intro

<div class="yarn-node" data-title="chest_intro">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: intro</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if has_item("key_gold")&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;inventory key_gold remove&gt;&gt;</span>
<span class="yarn-line">    Otwiera się! Jest w nim plecak.</span> <span class="yarn-meta">#line:0c1f006 </span>
    <span class="yarn-cmd">&lt;&lt;trigger chest_intro_open&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;SetInteractable chest_intro false&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;SetActive backpack true&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Jest zamknięte. Może potrzebujesz klucza.</span> <span class="yarn-meta">#line:chest_locked </span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-item-backpack"></a>

## item_backpack

<div class="yarn-node" data-title="item_backpack">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">group: intro</span>
<span class="yarn-header-dim">actor: </span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card backpack&gt;&gt;</span>
<span class="yarn-line">W tym plecaku można przenosić jedzenie, wodę i mapę.</span> <span class="yarn-meta">#line:05e80cb </span>
<span class="yarn-cmd">&lt;&lt;inventory backpack add&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;SetActive this false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;action wear_backpack&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;task_end get_backpack&gt;&gt;</span>
<span class="yarn-line">Teraz możemy opuścić obóz bazowy i rozpocząć przygodę!</span> <span class="yarn-meta">#line:029f39c</span>
<span class="yarn-cmd">&lt;&lt;target npc_intro&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-npc-base1"></a>

## npc_base1

<div class="yarn-node" data-title="npc_base1">
<pre class="yarn-code" style="--node-color:blue"><code>
<span class="yarn-header-dim">// ---------------------------------------</span>
<span class="yarn-header-dim">// Base 1 - The Valley's End</span>
<span class="yarn-header-dim">// ---------------------------------------</span>
<span class="yarn-header-dim">group: base_1</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if has_item("coat") and has_item("hat")&gt;&gt;</span>
<span class="yarn-line">    Jesteś teraz gotowy do wspinaczki!</span> <span class="yarn-meta">#line:015bf8a</span>
    <span class="yarn-cmd">&lt;&lt;camera_focus camera_snowman&gt;&gt;</span>
<span class="yarn-line">    Jeśli masz blokadę, bałwan może Ci pomóc!</span> <span class="yarn-meta">#line:showman_help</span>
    <span class="yarn-cmd">&lt;&lt;camera_reset&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $step_base1 = 1&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;SetActive door_base_1 false&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;target npc_base_2&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif !has_item("coat")&gt;&gt;</span>
<span class="yarn-line">    Brakuje Ci płaszcza!</span> <span class="yarn-meta">#line:03dd802 </span>
    <span class="yarn-cmd">&lt;&lt;target coat&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card wind&gt;&gt;</span>
<span class="yarn-line">    Zaczyna wiać wiatr. Moje uszy potrzebują ochrony!</span> <span class="yarn-meta">#line:b1_1</span>
<span class="yarn-line">    Jak chronić uszy, gdy wieje silny wiatr?</span> <span class="yarn-meta">#line:b1_q</span>
    <span class="yarn-cmd">&lt;&lt;activity fr08_activity_1 activity_1_done&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-activity-1-done"></a>

## activity_1_done

<div class="yarn-node" data-title="activity_1_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: base_1</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Otwórz skrzynię i wyjmij ciepłą czapkę!</span> <span class="yarn-meta">#line:0e83480 #task:get_hat</span>
<span class="yarn-cmd">&lt;&lt;set $step_base1 = 1&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;task_start get_hat&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;SetActive key_gold_1 true&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;target chest_1&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-chest-base1"></a>

## chest_base1

<div class="yarn-node" data-title="chest_base1">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: base_1</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if has_item("key_gold")&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;inventory key_gold remove&gt;&gt;</span>
<span class="yarn-line">    Otwiera się! Zawiera trochę sprzętu.</span> <span class="yarn-meta">#line:056ec2b </span>
    <span class="yarn-cmd">&lt;&lt;trigger chest_1_open&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;SetInteractable chest_1 false&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;SetActive hat true&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Jest zamknięte. Może potrzebujesz klucza.</span> <span class="yarn-meta">#shadow:chest_locked </span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-item-coat"></a>

## item_coat

<div class="yarn-node" data-title="item_coat">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">actor: </span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card coat&gt;&gt;</span>
<span class="yarn-line">Ten płaszcz zapewni Ci ciepło w czasie wiatru i śniegu.</span> <span class="yarn-meta">#line:0e23f1f </span>
<span class="yarn-cmd">&lt;&lt;inventory coat add&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;SetActive this false&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-item-hat"></a>

## item_hat

<div class="yarn-node" data-title="item_hat">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card hat&gt;&gt;</span>
<span class="yarn-line">Ciepła czapka utrzymuje ciepło głowy.</span> <span class="yarn-meta">#line:000fc05 </span>
<span class="yarn-line">Pomaga również, gdy wieje zimny wiatr.</span> <span class="yarn-meta">#line:059d744</span>
<span class="yarn-cmd">&lt;&lt;inventory hat add&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;SetActive this false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;task_end get_hat&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;target npc_base_1&gt;&gt;</span>
<span class="yarn-line">Teraz możesz kontynuować wspinaczkę!</span> <span class="yarn-meta">#line:0a7146f </span>

</code>
</pre>
</div>

<a id="ys-node-npc-base2"></a>

## npc_base2

<div class="yarn-node" data-title="npc_base2">
<pre class="yarn-code" style="--node-color:blue"><code>
<span class="yarn-header-dim">// ---------------------------------------</span>
<span class="yarn-header-dim">// Base 2 - Lower Mid</span>
<span class="yarn-header-dim">// ---------------------------------------</span>
<span class="yarn-header-dim">group: base_2</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if has_item("sunglasses") and has_item("gloves")&gt;&gt;</span>
<span class="yarn-line">    Jesteś teraz gotowy do wspinaczki!</span> <span class="yarn-meta">#shadow:015bf8a</span>
<span class="yarn-line">    Jeśli utkniesz, bałwan może Ci pomóc!</span> <span class="yarn-meta">#line:fr08_b2_help</span>
    <span class="yarn-cmd">&lt;&lt;set $step_base2 = 1&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;SetActive door_base_2 false&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;target npc_base_3&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif GetCurrentTask() == "get_sunglasses"&gt;&gt;</span>
<span class="yarn-line">    Wróć, gdy skończysz zadanie.</span> <span class="yarn-meta">#shadow:046eb33 </span>
<span class="yarn-cmd">&lt;&lt;elseif !has_item("sunglasses")&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card sun&gt;&gt;</span>
<span class="yarn-line">    Słońce mocno świeci na śniegu. Potrzebujemy ochrony oczu.</span> <span class="yarn-meta">#line:029a03e </span>
    <span class="yarn-cmd">&lt;&lt;card snow&gt;&gt;</span>
<span class="yarn-line">    Co chroni Twoje oczy przed jaskrawym śniegiem i słońcem?</span> <span class="yarn-meta">#line:0a70f7d </span>
    <span class="yarn-cmd">&lt;&lt;activity fr08_activity_2 activity_2_done&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif !has_item("gloves")&gt;&gt;</span>
<span class="yarn-line">    Brakuje Ci rękawiczek!</span> <span class="yarn-meta">#line:06b3363 </span>
    <span class="yarn-cmd">&lt;&lt;target gloves&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Szukaj dalej. Nadal potrzebujemy całego sprzętu.</span> <span class="yarn-meta">#line:keep_looking</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-activity-2-done"></a>

## activity_2_done

<div class="yarn-node" data-title="activity_2_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: base_2</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Otwórz skrzynię i wyjmij okulary przeciwsłoneczne!</span> <span class="yarn-meta">#line:0fdc3cb </span>
<span class="yarn-cmd">&lt;&lt;set $step_base2 = 1&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;task_start get_sunglasses&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;SetActive key_gold_2 true&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;target chest_2&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-chest-base2"></a>

## chest_base2

<div class="yarn-node" data-title="chest_base2">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: base_2</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if has_item("key_gold")&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;inventory key_gold remove&gt;&gt;</span>
<span class="yarn-line">    Otwiera się! W środku są okulary przeciwsłoneczne.</span> <span class="yarn-meta">#line:fr08_b2_chest </span>
    <span class="yarn-cmd">&lt;&lt;trigger chest_2_open&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;SetInteractable chest_2 false&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;SetActive sunglasses true&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Jest zamknięte. Może potrzebujesz klucza.</span> <span class="yarn-meta">#shadow:chest_locked</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-item-gloves"></a>

## item_gloves

<div class="yarn-node" data-title="item_gloves">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card gloves&gt;&gt;</span>
<span class="yarn-line">Rękawiczki utrzymują palce w cieple i suchości.</span> <span class="yarn-meta">#line:0f2aac7 </span>
<span class="yarn-line">Zimne ręce utrudniają wspinaczkę.</span> <span class="yarn-meta">#line:0d01879 </span>
<span class="yarn-cmd">&lt;&lt;inventory gloves add&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;SetActive this false&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-item-sunglasses"></a>

## item_sunglasses

<div class="yarn-node" data-title="item_sunglasses">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card ice&gt;&gt;</span>
<span class="yarn-line">Śnieg i lód odbijają jasne światło słoneczne.</span> <span class="yarn-meta">#line:02a2ea9 </span>
<span class="yarn-cmd">&lt;&lt;card sunglasses&gt;&gt;</span>
<span class="yarn-line">Okulary przeciwsłoneczne chronią oczy.</span> <span class="yarn-meta">#line:088fb7f</span>
<span class="yarn-cmd">&lt;&lt;inventory sunglasses add&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;SetActive this false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;task_end get_sunglasses&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;target npc_base_2&gt;&gt;</span>
<span class="yarn-line">Teraz możesz kontynuować wspinaczkę!</span> <span class="yarn-meta">#shadow:0a7146f </span>

</code>
</pre>
</div>

<a id="ys-node-npc-base3"></a>

## npc_base3

<div class="yarn-node" data-title="npc_base3">
<pre class="yarn-code" style="--node-color:blue"><code>
<span class="yarn-header-dim">// ---------------------------------------</span>
<span class="yarn-header-dim">// Base 3 - High-Mid</span>
<span class="yarn-header-dim">// ---------------------------------------</span>
<span class="yarn-header-dim">group: base_3</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if has_item("crampons") and has_item("rope")&gt;&gt;</span>
<span class="yarn-line">    Masz sprzęt lodowy. Ostatni atak na szczyt!</span> <span class="yarn-meta">#line:06b15bb </span>
    <span class="yarn-cmd">&lt;&lt;set $step_base3 = 1&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;SetActive door_base_3 false&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;target npc_base4&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif GetCurrentTask() == "get_crampons"&gt;&gt;</span>
<span class="yarn-line">    Wróć, gdy skończysz zadanie.</span> <span class="yarn-meta">#shadow:046eb33 </span>
<span class="yarn-cmd">&lt;&lt;elseif !has_item("rope")&gt;&gt;</span>
<span class="yarn-line">    Brakuje ci liny!</span> <span class="yarn-meta">#line:0c18c08 </span>
    <span class="yarn-cmd">&lt;&lt;target rope&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card ice&gt;&gt;</span>
<span class="yarn-line">    Aby chodzić po lodowcu, potrzebujemy specjalnego sprzętu.</span> <span class="yarn-meta">#line:06d9f34 </span>
    <span class="yarn-cmd">&lt;&lt;card glacier&gt;&gt;</span>
<span class="yarn-line">    Co zakładasz do butów, żeby chodzić po lodzie i się nie ślizgać?</span> <span class="yarn-meta">#line:076433f </span>
<span class="yarn-line">    Raki</span> <span class="yarn-meta">#line:0b50050 </span>
<span class="yarn-line">        Dobrze! Użyj ich, żeby utrzymać równowagę.</span> <span class="yarn-meta">#line:00a7e83 </span>
        <span class="yarn-cmd">&lt;&lt;activity fr08_activity_3 activity_3_done&gt;&gt;</span>
<span class="yarn-line">    Płetwy</span> <span class="yarn-meta">#line:01a4b20 </span>
<span class="yarn-line">        Nie! Nie pływasz! Spróbuj ponownie.</span> <span class="yarn-meta">#line:07c6385 </span>
        <span class="yarn-cmd">&lt;&lt;jump npc_base3&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-activity-3-done"></a>

## activity_3_done

<div class="yarn-node" data-title="activity_3_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: base_3</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Otwórz skrzynię, aby wyjąć raki!</span> <span class="yarn-meta">#line:02c9805 </span>
<span class="yarn-cmd">&lt;&lt;set $step_base3 = 1&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;task_start get_crampons&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;SetActive key_gold_3 true&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;target chest_3&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-chest-base3"></a>

## chest_base3

<div class="yarn-node" data-title="chest_base3">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: base_3</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if has_item("key_gold")&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;inventory key_gold remove&gt;&gt;</span>
<span class="yarn-line">    Otwiera się! Zawiera trochę sprzętu.</span> <span class="yarn-meta">#shadow:056ec2b </span>
    <span class="yarn-cmd">&lt;&lt;trigger chest_3_open&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;SetInteractable chest_3 false&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;SetActive crampons true&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Jest zamknięte. Może potrzebujesz klucza.</span> <span class="yarn-meta">#shadow:chest_locked</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-item-crampons"></a>

## item_crampons

<div class="yarn-node" data-title="item_crampons">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card crampons&gt;&gt;</span>
<span class="yarn-line">Pomagają bezpiecznie chodzić po lodzie.</span> <span class="yarn-meta">#line:07102e7 </span>
<span class="yarn-cmd">&lt;&lt;inventory crampons add&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;SetActive this false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;task_end get_crampons&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;target npc_base3&gt;&gt;</span>
<span class="yarn-line">Teraz możesz kontynuować wspinaczkę!</span> <span class="yarn-meta">#shadow:0a7146f </span>

</code>
</pre>
</div>

<a id="ys-node-item-rope"></a>

## item_rope

<div class="yarn-node" data-title="item_rope">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card rope&gt;&gt;</span>
<span class="yarn-line">Lina pomaga wspinaczom zachować bezpieczeństwo na lodzie i skałach.</span> <span class="yarn-meta">#line:038d1bc </span>
<span class="yarn-cmd">&lt;&lt;inventory rope add&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;SetActive this false&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-npc-base4"></a>

## npc_base4

<div class="yarn-node" data-title="npc_base4">
<pre class="yarn-code" style="--node-color:blue"><code>
<span class="yarn-header-dim">// ---------------------------------------</span>
<span class="yarn-header-dim">// Base 4 - The Top</span>
<span class="yarn-header-dim">// ---------------------------------------</span>
<span class="yarn-header-dim">group: base_4</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card summit&gt;&gt;</span>
<span class="yarn-line">Udało się! Jesteśmy na szczycie Mont Blanc.</span> <span class="yarn-meta">#line:030fa1c </span>
<span class="yarn-line">Wysokość tego szczytu wynosi 4807 metrów.</span> <span class="yarn-meta">#line:018880a </span>
<span class="yarn-line">Przed grą o flagę odpowiedzmy na trzy pytania dotyczące gór.</span> <span class="yarn-meta">#line:001e940 </span>
<span class="yarn-cmd">&lt;&lt;jump question_alps&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-question-alps"></a>

## question_alps

<div class="yarn-node" data-title="question_alps">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: base_4</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">W jakim paśmie górskim znajduje się Mont Blanc?</span> <span class="yarn-meta">#line:06f9a2a </span>
<span class="yarn-line">Pireneje</span> <span class="yarn-meta">#line:summit_q1_a2</span>
<span class="yarn-line">    Nie do końca. Spróbuj ponownie.</span> <span class="yarn-meta">#line:try_again</span>
    <span class="yarn-cmd">&lt;&lt;jump question_alps&gt;&gt;</span>
<span class="yarn-line">Apeniny</span> <span class="yarn-meta">#line:summit_q1_a3</span>
<span class="yarn-line">    Nie do końca. Spróbuj ponownie.</span> <span class="yarn-meta">#shadow:try_again</span>
    <span class="yarn-cmd">&lt;&lt;jump question_alps&gt;&gt;</span>
<span class="yarn-line">Alpy</span> <span class="yarn-meta">#line:summit_q1_a1</span>
<span class="yarn-line">    Prawidłowy!</span> <span class="yarn-meta">#line:correct</span>
    <span class="yarn-cmd">&lt;&lt;jump question_height&gt;&gt;</span>
<span class="yarn-line">Nie wiem</span> <span class="yarn-meta">#line:dontknow #highlight</span>
<span class="yarn-line">    Wróć później!</span> <span class="yarn-meta">#line:comebacklater</span>

</code>
</pre>
</div>

<a id="ys-node-question-height"></a>

## question_height

<div class="yarn-node" data-title="question_height">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: base_4</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Jak wysoki jest Mont Blanc?</span> <span class="yarn-meta">#line:summit_q2</span>
<span class="yarn-line">3705 metrów</span> <span class="yarn-meta">#line:summit_q2_a2</span>
<span class="yarn-line">    Nie do końca. Spróbuj ponownie.</span> <span class="yarn-meta">#shadow:try_again</span>
    <span class="yarn-cmd">&lt;&lt;jump question_height&gt;&gt;</span>
<span class="yarn-line">4807 metrów</span> <span class="yarn-meta">#line:summit_q2_a1</span>
<span class="yarn-line">    Prawidłowy!</span> <span class="yarn-meta">#shadow:correct</span>
    <span class="yarn-cmd">&lt;&lt;jump question_summit_flags&gt;&gt;</span>
<span class="yarn-line">5016 metrów</span> <span class="yarn-meta">#line:summit_q2_a3</span>
<span class="yarn-line">    Nie do końca. Spróbuj ponownie.</span> <span class="yarn-meta">#shadow:try_again</span>
    <span class="yarn-cmd">&lt;&lt;jump question_height&gt;&gt;</span>
<span class="yarn-line">Nie wiem</span> <span class="yarn-meta">#shadow:dontknow #highlight</span>
<span class="yarn-line">    Wróć później!</span> <span class="yarn-meta">#shadow:comebacklater</span>

</code>
</pre>
</div>

<a id="ys-node-question-summit-flags"></a>

## question_summit_flags

<div class="yarn-node" data-title="question_summit_flags">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: base_4</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Jakie kraje widać stąd?</span> <span class="yarn-meta">#line:summit_q</span>
<span class="yarn-line">Francja, Hiszpania i Niemcy</span> <span class="yarn-meta">#line:summit_a2</span>
<span class="yarn-line">    Nie do końca. To trochę za daleko!</span> <span class="yarn-meta">#line:summit_no</span>
    <span class="yarn-cmd">&lt;&lt;jump question_summit_flags&gt;&gt;</span>
<span class="yarn-line">Francja, Włochy i Szwajcaria</span> <span class="yarn-meta">#line:summit_a1</span>
<span class="yarn-line">    Dobrze! Rozwieśmy flagi.</span> <span class="yarn-meta">#line:summit_ok</span>
    <span class="yarn-cmd">&lt;&lt;activity fr08_match_flags quest_end&gt;&gt;</span>
<span class="yarn-line">Alpy</span> <span class="yarn-meta">#shadow:summit_q1_a1</span>
<span class="yarn-line">    Wróć później!</span> <span class="yarn-meta">#shadow:comebacklater</span>
<span class="yarn-line">Nie wiem</span> <span class="yarn-meta">#shadow:dontknow #highlight</span>
<span class="yarn-line">    Wróć później!</span> <span class="yarn-meta">#shadow:comebacklater</span>

</code>
</pre>
</div>

<a id="ys-node-npc-france"></a>

## npc_france

<div class="yarn-node" data-title="npc_france">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card country_france&gt;&gt;</span>
<span class="yarn-line">Francja leży na zachód od Mont Blanc.</span> <span class="yarn-meta">#line:0d23529 </span>

</code>
</pre>
</div>

<a id="ys-node-npc-italy"></a>

## npc_italy

<div class="yarn-node" data-title="npc_italy">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card country_italy&gt;&gt;</span>
<span class="yarn-line">Włochy leżą na wschód od Mont Blanc.</span> <span class="yarn-meta">#line:050fe70  </span>

</code>
</pre>
</div>

<a id="ys-node-npc-swiss"></a>

## npc_swiss

<div class="yarn-node" data-title="npc_swiss">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card country_switzerland&gt;&gt;</span>
<span class="yarn-line">Szwajcaria leży na północ od Mont Blanc.</span> <span class="yarn-meta">#line:03db010  </span>

</code>
</pre>
</div>

<a id="ys-node-snowman-1"></a>

## snowman_1

<div class="yarn-node" data-title="snowman_1">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">// ---------------------------------------</span>
<span class="yarn-header-dim">// AREA BONUS</span>
<span class="yarn-header-dim">// ---------------------------------------</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if HasCompletedTask("get_hat")&gt;&gt;</span>
<span class="yarn-line">    Potrzebujesz pomocy we wspięciu się na górę?</span> <span class="yarn-meta">#line:0c18060 </span>
<span class="yarn-line">    Tak</span> <span class="yarn-meta">#line:0f4c405 </span>
        <span class="yarn-cmd">&lt;&lt;teleport base_2&gt;&gt;</span>
<span class="yarn-line">    Nie, zostaję tutaj</span> <span class="yarn-meta">#line:02a3874</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Wróć później!</span> <span class="yarn-meta">#shadow:comebacklater</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-snowman-2"></a>

## snowman_2

<div class="yarn-node" data-title="snowman_2">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if has_item("gloves")&gt;&gt;</span>
<span class="yarn-line">    Potrzebujesz pomocy we wspięciu się na górę?</span> <span class="yarn-meta">#shadow:0c18060 </span>
<span class="yarn-line">    Tak</span> <span class="yarn-meta">#shadow:0f4c405 </span>
        <span class="yarn-cmd">&lt;&lt;teleport base_3&gt;&gt;</span>
<span class="yarn-line">    Nie, zostaję tutaj</span> <span class="yarn-meta">#shadow:02a3874 </span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Wróć później!</span> <span class="yarn-meta">#shadow:comebacklater</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-snowman-3"></a>

## snowman_3

<div class="yarn-node" data-title="snowman_3">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if HasCompletedTask("get_sunglasses")&gt;&gt;</span>
<span class="yarn-line">    Potrzebujesz pomocy we wspięciu się na górę?</span> <span class="yarn-meta">#shadow:0c18060 </span>
<span class="yarn-line">    Tak</span> <span class="yarn-meta">#shadow:0f4c405 </span>
        <span class="yarn-cmd">&lt;&lt;teleport base_4&gt;&gt;</span>
<span class="yarn-line">    Nie, zostaję tutaj</span> <span class="yarn-meta">#shadow:02a3874 </span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Wróć później!</span> <span class="yarn-meta">#shadow:comebacklater</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-snowman-4"></a>

## snowman_4

<div class="yarn-node" data-title="snowman_4">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Potrzebujesz pomocy we wspięciu się na górę?</span> <span class="yarn-meta">#shadow:0c18060 </span>
<span class="yarn-line">Tak</span> <span class="yarn-meta">#shadow:0f4c405 </span>
    <span class="yarn-cmd">&lt;&lt;teleport base_5&gt;&gt;</span>
<span class="yarn-line">Nie, zostaję tutaj</span> <span class="yarn-meta">#shadow:02a3874 </span>

</code>
</pre>
</div>

<a id="ys-node-snowman-5"></a>

## snowman_5

<div class="yarn-node" data-title="snowman_5">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if HasCompletedTask("get_crampons")&gt;&gt;</span>
<span class="yarn-line">    Potrzebujesz pomocy we wspięciu się na górę?</span> <span class="yarn-meta">#shadow:0c18060 </span>
<span class="yarn-line">    Tak</span> <span class="yarn-meta">#shadow:0f4c405 </span>
        <span class="yarn-cmd">&lt;&lt;teleport base_6&gt;&gt;</span>
<span class="yarn-line">    Nie, zostaję tutaj</span> <span class="yarn-meta">#shadow:02a3874 </span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Wróć później!</span> <span class="yarn-meta">#shadow:comebacklater</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-snowman-6"></a>

## snowman_6

<div class="yarn-node" data-title="snowman_6">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Potrzebujesz pomocy we wspięciu się na górę?</span> <span class="yarn-meta">#shadow:0c18060 </span>
<span class="yarn-line">Tak</span> <span class="yarn-meta">#shadow:0f4c405 </span>
    <span class="yarn-cmd">&lt;&lt;teleport base_7&gt;&gt;</span>
<span class="yarn-line">Nie, zostaję tutaj</span> <span class="yarn-meta">#shadow:02a3874 </span>

</code>
</pre>
</div>

<a id="ys-node-snowman-7"></a>

## snowman_7

<div class="yarn-node" data-title="snowman_7">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Potrzebujesz pomocy we wspięciu się na górę?</span> <span class="yarn-meta">#shadow:0c18060 </span>
<span class="yarn-line">Tak</span> <span class="yarn-meta">#shadow:0f4c405 </span>
    <span class="yarn-cmd">&lt;&lt;teleport base_1&gt;&gt;</span>
<span class="yarn-line">Nie, zostaję tutaj</span> <span class="yarn-meta">#shadow:02a3874 </span>

</code>
</pre>
</div>

<a id="ys-node-marmot"></a>

## marmot

<div class="yarn-node" data-title="marmot">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">group: animals</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-comment">// The marmot makes a sound effect before talking</span>
<span class="yarn-comment">// &lt;&lt;play_sfx marmot_whistle&gt;&gt;</span>
<span class="yarn-line">Całą zimę śpię pod śniegiem. To się nazywa hibernacja!</span> <span class="yarn-meta">#line:fr08_marmot_2 #card:marmot</span>
<span class="yarn-line">Uwielbiam jeść alpejskie kwiaty i trawę. Pycha!</span> <span class="yarn-meta">#line:fr08_marmot_3 #card:marmot</span>
<span class="yarn-line">Nie zwracajcie na mnie uwagi, po prostu opalam się na tej ciepłej skale.</span> <span class="yarn-meta">#line:fr08_marmot_4 #card:marmot</span>

</code>
</pre>
</div>

<a id="ys-node-chest-bonus"></a>

## chest_bonus

<div class="yarn-node" data-title="chest_bonus">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: bonus</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if has_item("key_gold")&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;inventory key_gold remove&gt;&gt;</span>
<span class="yarn-line">    Znalazłeś ciepły szalik!</span> <span class="yarn-meta">#line:003e87c </span>
    <span class="yarn-cmd">&lt;&lt;trigger chest_bonus_open&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;SetInteractable chest_bonus false&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;SetActive scarf true&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Jest zamknięte. Może potrzebujesz klucza.</span> <span class="yarn-meta">#shadow:chest_locked</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-item-scarf"></a>

## item_scarf

<div class="yarn-node" data-title="item_scarf">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">group: bonus</span>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card scarf&gt;&gt;</span>
<span class="yarn-line">Szalik chroni przed wiatrem na szyi.</span> <span class="yarn-meta">#line:0afb2dc </span>
<span class="yarn-cmd">&lt;&lt;inventory scarf add&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;SetActive this false&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-spawned-tourist"></a>

## spawned_tourist

<div class="yarn-node" data-title="spawned_tourist">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">// ---------------------------------------</span>
<span class="yarn-header-dim">// NPCs SPAWNED IN THE SCENE</span>
<span class="yarn-header-dim">// these npc are spawn automatically in the scene</span>
<span class="yarn-header-dim">// use these to add random facts. everythime you meet them</span>
<span class="yarn-header-dim">// they will say one of these lines randomly</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: ADULT_F</span>
<span class="yarn-header-dim">spawn_group: tourists </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Uwielbiam góry!</span> <span class="yarn-meta">#line:011dc7e </span>
<span class="yarn-line">Mont Blanc jest najwyższą górą w Alpach!</span> <span class="yarn-meta">#line:003ecb2 </span>
<span class="yarn-line">Mam nadzieję zobaczyć świstaka!</span> <span class="yarn-meta">#line:04bbada #card:marmot</span>
<span class="yarn-line">Nie zapomnij zabrać ciepłych ubrań!</span> <span class="yarn-meta">#line:0590129 </span>
<span class="yarn-line">Góry mogą być niebezpieczne, więc bądź ostrożny!</span> <span class="yarn-meta">#line:097ecf0 </span>

</code>
</pre>
</div>

<a id="ys-node-spawned-hiker"></a>

## spawned_hiker

<div class="yarn-node" data-title="spawned_hiker">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: ADULT_M</span>
<span class="yarn-header-dim">spawn_group: hikers</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Do pieszych wędrówek tutaj potrzebne są dobre buty.</span> <span class="yarn-meta">#line:0dbac73 #card:hiking</span>
<span class="yarn-line">Wiatr może się szybko zmienić.</span> <span class="yarn-meta">#line:0557ef5 #card:wind</span>
<span class="yarn-line">Gdy jest ślisko, podążam za liną.</span> <span class="yarn-meta">#line:0bc67f7 #card:rope</span>
<span class="yarn-line">Ten lodowiec wygląda jak zamarznięta rzeka.</span> <span class="yarn-meta">#line:01e5f66 #card:glacier</span>

</code>
</pre>
</div>

<a id="ys-node-spawned-alps-climber"></a>

## spawned_alps_climber

<div class="yarn-node" data-title="spawned_alps_climber">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: ADULT_F</span>
<span class="yarn-header-dim">spawn_group: alps_climber</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Uwielbiam wspinaczkę w Alpach.</span> <span class="yarn-meta">#line:0809dce #card:alps</span>
<span class="yarn-line">Raki pomagają mi chodzić po twardym lodzie.</span> <span class="yarn-meta">#line:081fbe4 #card:crampons</span>
<span class="yarn-line">Wspinaczka z przewodnikiem jest bezpieczniejsza.</span> <span class="yarn-meta">#line:0371d31 #card:mountain_guide</span>

</code>
</pre>
</div>


