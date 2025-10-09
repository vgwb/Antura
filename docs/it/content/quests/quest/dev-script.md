---
title: DEV (dev) - Script
hide:
---

# DEV (dev) - Script
> [!note] Educators & Designers: help improving this quest!
> **Comments and feedback**: [discuss in the Forum]()  
> **Improve translations**: [comment the Google Sheet]()  
> **Improve the script**: [propose an edit here](https://github.com/vgwb/Antura/blob/main/Assets/_discover/_quests/_DEV/DEV%20-%20Yarn%20Script.yarn)  

<a id="ys-node-quest-start"></a>

## quest_start

<div class="yarn-node" data-title="quest_start">
<pre class="yarn-code" style="--node-color:red"><code>
<span class="yarn-header-dim">// dev | DEV</span>
<span class="yarn-header-dim">// </span>
<span class="yarn-header-dim">// Country: Italy - Firenze</span>
<span class="yarn-header-dim">// Content: Living document for testing Yarn features</span>
<span class="yarn-header-dim">// and documenting available commands</span>
<span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">// INIT</span>
<span class="yarn-header-dim">// every quest starts with an INIT node</span>
<span class="yarn-header-dim">// to inizialize existing variables and declare new ones</span>
<span class="yarn-header-dim">// title of a node must be unique to this quest</span>
<span class="yarn-header-dim">// create a new node for anything that must be referenced multiple times</span>
<span class="yarn-header-dim">// that makes understanding of the flow easier</span>
<span class="yarn-header-dim">// position is used to place the node in the Graph view in the Editor</span>
<span class="yarn-header-dim">// group is used to organize the nodes in the Graph view in the Editor</span>
<span class="yarn-header-dim">group: docs</span>

<span class="yarn-header-dim">// tags are used to add metadata to the node</span>
<span class="yarn-header-dim">// actor is the speaking character, used mostly for voice overs</span>
<span class="yarn-header-dim">// - Default | female, neutral, clear diction, mid pitch, minimal emotion</span>
<span class="yarn-header-dim">// - SILENT | no spoken words, but can use non-verbal sounds</span>
<span class="yarn-header-dim">// - NARRATOR | male, warm, engaging, enthusiastic</span>
<span class="yarn-header-dim">// - SPECIAL | fx voice, like a robot or an animal</span>
<span class="yarn-header-dim">// - ADULT_F | warm, conversational, adaptable</span>
<span class="yarn-header-dim">// - ADULT_M | natural, medium pitch, flexible tone</span>
<span class="yarn-header-dim">// - SENIOR_F | warm, gentle rasp, storyteller cadence</span>
<span class="yarn-header-dim">// - SENIOR_M | deeper, slower, authoritative but kind</span>
<span class="yarn-header-dim">// - KID_F | bright, energetic, quick speech, high pitch</span>
<span class="yarn-header-dim">// - KID_M | playful, curious, lively tone, mid-high pitch</span>

<span class="yarn-header-dim">actor: NARRATOR</span>

<span class="yarn-header-dim">// color us used like this:</span>
<span class="yarn-header-dim">//  - red for important nodes like init and quest end</span>
<span class="yarn-header-dim">//  - blue for NPCs</span>
<span class="yarn-header-dim">//  - yellow for items</span>
<span class="yarn-header-dim">//  - green for tasks</span>
<span class="yarn-header-dim">//  - purple for activities</span>
<span class="yarn-header-dim">color: red</span>

<span class="yarn-header-dim">// type is used to identify special nodes like panel, Quiz</span>
<span class="yarn-header-dim">// - panel is fullscreen</span>
<span class="yarn-header-dim">// - panel_endgame is a special panel that shows the endgame screen</span>
<span class="yarn-header-dim">// - quiz shows a quiz with image choiches</span>
<span class="yarn-header-dim">type: panel</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-comment">// these are the common variables</span>
<span class="yarn-cmd">&lt;&lt;set $EASY_MODE = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;set $IS_DESKTOP = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;set $TOTAL_COINS = 0&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;set $COLLECTED_ITEMS = 0&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;set $MAX_PROGRESS = 0&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;set $CURRENT_PROGRESS = 0&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;set $CURRENT_ITEM = ""&gt;&gt;</span>

<span class="yarn-comment">// here we declare new variables for this quest</span>
<span class="yarn-cmd">&lt;&lt;declare $doorUnlocked = false&gt;&gt;</span>

<span class="yarn-line">Benvenuti alla missione DEV!</span> <span class="yarn-meta">#line:0973921 </span>
<span class="yarn-cmd">&lt;&lt;card food_baguette&gt;&gt;</span>
<span class="yarn-line">Puoi usare questa missione per testare funzionalità e comandi.</span> <span class="yarn-meta">#line:0196a6d </span>
<span class="yarn-cmd">&lt;&lt;asset backpack&gt;&gt;</span>
<span class="yarn-comment">// shadow line example</span>
[MISSING TRANSLATION: You played the activity well!]

</code>
</pre>
</div>

<a id="ys-node-docs-global-variable"></a>

## DOCS_GLOBAL_VARIABLE

<div class="yarn-node" data-title="DOCS_GLOBAL_VARIABLE">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: docs</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $EASY_MODE &gt;&gt;</span>
    [MISSING TRANSLATION:     You are in EASY MODE!]
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>    
    [MISSING TRANSLATION:     You are in NORMAL MODE!]
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;if $IS_DESKTOP &gt;&gt;</span>
    [MISSING TRANSLATION:     You are on DESKTOP!]
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
    [MISSING TRANSLATION:     You are on MOBILE!]
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-docs-actions"></a>

## DOCS_ACTIONS

<div class="yarn-node" data-title="DOCS_ACTIONS">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">// DOCS COMMANDS</span>
<span class="yarn-header-dim">// these are the commands available in all Scripts</span>
<span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">group: docs</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-comment">// execute the configured action in the actionManager</span>
<span class="yarn-cmd">&lt;&lt;action action_id&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-docs-activity"></a>

## DOCS_ACTIVITY

<div class="yarn-node" data-title="DOCS_ACTIVITY">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: docs</span>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-comment">// launches an activity and set a yarn node to jump when completed</span>
<span class="yarn-cmd">&lt;&lt;activity activity_setting_id return_node&gt;&gt;</span>

<span class="yarn-comment">// a third optional parameter can set the difficulty</span>
<span class="yarn-cmd">&lt;&lt;activity activity_setting_id return_node tutorial&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;activity activity_setting_id return_node easy&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;activity activity_setting_id return_node normal&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;activity activity_setting_id return_node expert&gt;&gt;</span>

<span class="yarn-comment">// to get the result of a specific activity</span>
<span class="yarn-comment">// (0 = failed, &gt;= 1 = completed)</span>
&lt;&lt;if GetActivityResult("id_activity_setting") &gt; 0&gt;&gt;
<span class="yarn-line">Hai svolto bene l'attività!</span> <span class="yarn-meta">#line:0872c3a </span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

<span class="yarn-comment">// this returns the result of the activity played</span>
&lt;&lt;if GetActivityResult("") &gt; 0&gt;&gt;
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-docs-area"></a>

## DOCS_AREA

<div class="yarn-node" data-title="DOCS_AREA">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: docs</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-comment">// set the area (walls), turnng off the previous one</span>
<span class="yarn-cmd">&lt;&lt;area full_area&gt;&gt;</span>
<span class="yarn-comment">// switch off the current area.</span>
<span class="yarn-cmd">&lt;&lt;area off&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-docs-assets"></a>

## DOCS_ASSETS

<div class="yarn-node" data-title="DOCS_ASSETS">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: docs</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-comment">// insted of a card, uses a direct Asset Id</span>
<span class="yarn-cmd">&lt;&lt;asset id_asset&gt;&gt;</span> 
<span class="yarn-comment">// hides the current image</span>
<span class="yarn-cmd">&lt;&lt;asset_hide&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-docs-camera"></a>

## DOCS_CAMERA

<div class="yarn-node" data-title="DOCS_CAMERA">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: docs</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-comment">// focus the camera on a target (interactable, area, map point)</span>
<span class="yarn-cmd">&lt;&lt;camera_focus palace&gt;&gt;</span>
<span class="yarn-comment">// back to player</span>
<span class="yarn-cmd">&lt;&lt;camera_reset&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-docs-cards"></a>

## DOCS_CARDS

<div class="yarn-node" data-title="DOCS_CARDS">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: docs</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-comment">// uses the card Id to show a card</span>
<span class="yarn-cmd">&lt;&lt;card food_baguette&gt;&gt;</span>
<span class="yarn-comment">// if accepts 2 additional parameters</span>
<span class="yarn-comment">// zoom - shows the card in zoom mode</span>
<span class="yarn-comment">// silent - shows the card without title / audio</span>
<span class="yarn-cmd">&lt;&lt;card food_baguette zoom silent&gt;&gt;</span>
<span class="yarn-comment">// obviously can set just one of them</span>
<span class="yarn-cmd">&lt;&lt;card food_baguette silent&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;card food_baguette zoom&gt;&gt;</span>

<span class="yarn-comment">// hides the current card</span>
<span class="yarn-cmd">&lt;&lt;card_hide&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-docs-cookies"></a>

## DOCS_COOKIES

<div class="yarn-node" data-title="DOCS_COOKIES">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: docs</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-comment">// adds cookies to the player (can be negative)</span>
<span class="yarn-cmd">&lt;&lt;cookies_add 5&gt;&gt;</span>

&lt;&lt;if GetCookies() &gt;= 10&gt;&gt;
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-docs-inventory"></a>

## DOCS_INVENTORY

<div class="yarn-node" data-title="DOCS_INVENTORY">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: docs</span>
<span class="yarn-header-dim">actor: ADULT_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-comment">// adds the item = card to the inventory (needs to be collectable!)</span>
<span class="yarn-cmd">&lt;&lt;inventory id_card add&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;inventory id_card&gt;&gt;</span> // same as add
<span class="yarn-cmd">&lt;&lt;inventory id_card remove&gt;&gt;</span>

<span class="yarn-comment">// Inventory has custom functions to check items</span>
&lt;&lt;if item_count("flag_france") &gt;= 2&gt;&gt;
<span class="yarn-line">    Hai più di 2 bandiere della Francia</span> <span class="yarn-meta">#line:08a1058 #native</span>
<span class="yarn-line">    un'altra linea</span> <span class="yarn-meta">#line:0563122 </span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

<span class="yarn-cmd">&lt;&lt;if has_item("flag_france")&gt;&gt;</span>
<span class="yarn-line">    Hai una bandiera della Francia</span> <span class="yarn-meta">#line:kidm_0b932d0 </span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

<span class="yarn-cmd">&lt;&lt;if has_item_at_least("flag_france", 3)&gt;&gt;</span>
<span class="yarn-line">    Hai almeno 3 bandiere della Francia</span> <span class="yarn-meta">#line:0d694f1 </span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

<span class="yarn-cmd">&lt;&lt;if can_collect("flag_france")&gt;&gt;</span>
<span class="yarn-line">    Puoi collezionare una bandiera della Francia</span> <span class="yarn-meta">#line:03da430 </span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>


</code>
</pre>
</div>

<a id="ys-node-docs-tasks"></a>

## DOCS_TASKS

<div class="yarn-node" data-title="DOCS_TASKS">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: docs</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-comment">// starts a task</span>
<span class="yarn-cmd">&lt;&lt;task_start id_task_configuration&gt;&gt;</span>
<span class="yarn-comment">// end a task... if you say nothing, its a success</span>
<span class="yarn-cmd">&lt;&lt;task_end id_task_configuration&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;task_end id_task_configuration fail&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;task_end&gt;&gt;</span> // or even without the id to end current task

</code>
</pre>
</div>

<a id="ys-node-docs-party"></a>

## DOCS_PARTY

<div class="yarn-node" data-title="DOCS_PARTY">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: docs</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-comment">// makes a NPC join a party following the player</span>
<span class="yarn-cmd">&lt;&lt;party_join interactable_id&gt;&gt;</span>
<span class="yarn-comment">// asks a NPC to leave the party</span>
<span class="yarn-cmd">&lt;&lt;party_release interactable_id&gt;&gt;</span>
<span class="yarn-comment">// if no id, remove all</span>
<span class="yarn-cmd">&lt;&lt;party_release&gt;&gt;</span> 
<span class="yarn-comment">// the default formation in "line", but other are available</span>
<span class="yarn-cmd">&lt;&lt;party_formation "line"&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;party_formation "circle"&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;party_formation "V"&gt;&gt;</span>


</code>
</pre>
</div>

<a id="ys-node-docs-quest"></a>

## DOCS_QUEST

<div class="yarn-node" data-title="DOCS_QUEST">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: docs</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-comment">// end current quest and calculate stars based on progress</span>
<span class="yarn-cmd">&lt;&lt;quest_end&gt;&gt;</span>
<span class="yarn-comment">// end the current quest with n (0, 1,2,3) stars</span>
<span class="yarn-cmd">&lt;&lt;quest_end 3&gt;&gt;</span>


</code>
</pre>
</div>

<a id="ys-node-docs-target"></a>

## DOCS_TARGET

<div class="yarn-node" data-title="DOCS_TARGET">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: docs</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-comment">// put a target element on a target</span>
<span class="yarn-cmd">&lt;&lt;target target_chest&gt;&gt;</span>
<span class="yarn-comment">// removes any target</span>
<span class="yarn-cmd">&lt;&lt;target off&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-docs-yarn-builtin"></a>

## DOCS_YARN_BUILTIN

<div class="yarn-node" data-title="DOCS_YARN_BUILTIN">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: docs</span>
<span class="yarn-header-dim">---</span>

<span class="yarn-comment">// --- Yarn built-in commands ---</span>
<span class="yarn-cmd">&lt;&lt;wait 3&gt;&gt;</span> // waits for 3 seconds
<span class="yarn-cmd">&lt;&lt;stop&gt;&gt;</span> // stops the current dialogue&gt;&gt;

</code>
</pre>
</div>

<a id="ys-node-test-dialog"></a>

## test_dialog

<div class="yarn-node" data-title="test_dialog">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: dialog</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card food_baguette&gt;&gt;</span>
<span class="yarn-line">BENVENUTO</span> <span class="yarn-meta">#line:0c6f99f </span>
<span class="yarn-cmd">&lt;&lt;card food_baguette zoom&gt;&gt;</span>
<span class="yarn-line">Seconda linea</span> <span class="yarn-meta">#line:06a2e0a </span>
<span class="yarn-line">Terza riga. Cosa vuoi?</span> <span class="yarn-meta">#line:0d95cfd </span>
<span class="yarn-line">Burro!</span> <span class="yarn-meta">#line:05edb53 </span>
    <span class="yarn-cmd">&lt;&lt;card butter&gt;&gt;</span>
<span class="yarn-line">    Buon burro</span> <span class="yarn-meta">#line:090e4bd </span>
<span class="yarn-line">Opzione 2</span> <span class="yarn-meta">#line:07243c2 </span>
<span class="yarn-line">    Hai scelto 2</span> <span class="yarn-meta">#line:03649cf </span>
<span class="yarn-line">Opzione 3</span> <span class="yarn-meta">#line:0c56f8c </span>
<span class="yarn-line">    Hai scelto 3</span> <span class="yarn-meta">#line:0278662 </span>
<span class="yarn-line">Opzione 4</span> <span class="yarn-meta">#line:0131912 </span>
<span class="yarn-line">    Hai scelto 4</span> <span class="yarn-meta">#line:077dcad </span>
<span class="yarn-line">E questo è pesce</span> <span class="yarn-meta">#line:099d213 </span>
<span class="yarn-cmd">&lt;&lt;card food_fish&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;card food_fish zoom&gt;&gt;</span>
<span class="yarn-line">il pesce è ancora qui</span> <span class="yarn-meta">#line:08da0cf </span>
<span class="yarn-line">Ci piace!</span> <span class="yarn-meta">#line:04d8ba8 </span>
<span class="yarn-line">Prima riga v1</span> <span class="yarn-meta">#line:04a8afc </span>
<span class="yarn-line">Prima riga v2</span> <span class="yarn-meta">#line:08cbb53 </span>


</code>
</pre>
</div>

<a id="ys-node-test-inventory"></a>

## test_inventory

<div class="yarn-node" data-title="test_inventory">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: inventory</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Ho fame. Che ne dici di qualcosa da mangiare?</span> <span class="yarn-meta">#line:0bdf566 </span>
<span class="yarn-cmd">&lt;&lt;if has_item_at_least("food_baguette", 3)&gt;&gt;</span>
<span class="yarn-line">    Mi servono 3 baguette!</span> <span class="yarn-meta">#line:00c341e </span>
    <span class="yarn-cmd">&lt;&lt;inventory food_baguette remove&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;inventory food_baguette remove&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;inventory food_baguette remove&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Non hai abbastanza baguette!</span> <span class="yarn-meta">#line:0c0d786 </span>
    <span class="yarn-cmd">&lt;&lt;if has_item("food_fish")&gt;&gt;</span>
<span class="yarn-line">        Ma hai dei pesci!</span> <span class="yarn-meta">#line:06c9370 </span>
    <span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>
<span class="yarn-line">    Grazie.</span> <span class="yarn-meta">#line:0a7921b </span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>


</code>
</pre>
</div>

<a id="ys-node-got-baguette"></a>

## got_baguette

<div class="yarn-node" data-title="got_baguette">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: inventory</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Hai trovato una baguette!</span> <span class="yarn-meta">#line:dev_got_baguette </span>
<span class="yarn-cmd">&lt;&lt;inventory food_baguette add&gt;&gt;</span>


</code>
</pre>
</div>

<a id="ys-node-got-fish"></a>

## got_fish

<div class="yarn-node" data-title="got_fish">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: inventory</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Un pesce sano!</span> <span class="yarn-meta">#line:dev_got_fish </span>
<span class="yarn-cmd">&lt;&lt;inventory food_fish add&gt;&gt;</span>


</code>
</pre>
</div>

<a id="ys-node-area-change"></a>

## area_change

<div class="yarn-node" data-title="area_change">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: area </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Ora ti lascio fare il giro.</span> <span class="yarn-meta">#line:0aa3b5d </span>
<span class="yarn-cmd">&lt;&lt;area full_area&gt;&gt;</span>


</code>
</pre>
</div>

<a id="ys-node-test-cards"></a>

## test_cards

<div class="yarn-node" data-title="test_cards">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: cards </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Adesso vi mostro alcune carte</span> <span class="yarn-meta">#line:05edfca </span>
<span class="yarn-cmd">&lt;&lt;card food_fish&gt;&gt;</span>
<span class="yarn-line">Per favore guarda i suoi dettagli</span> <span class="yarn-meta">#line:0adbe4f </span>
<span class="yarn-cmd">&lt;&lt;card food_fish zoom&gt;&gt;</span>
<span class="yarn-line">torna al menu e sfoglia le carte</span> <span class="yarn-meta">#line:017a620 </span>
<span class="yarn-line">e adesso...</span> <span class="yarn-meta">#line:04ade19 </span>
<span class="yarn-cmd">&lt;&lt;card food_baguette&gt;&gt;</span>
<span class="yarn-line">Bello, vero?</span> <span class="yarn-meta">#line:04b02ce </span>
<span class="yarn-cmd">&lt;&lt;card_hide&gt;&gt;</span>
<span class="yarn-line">Torna subito al lavoro.</span> <span class="yarn-meta">#line:0970ed1 </span>


</code>
</pre>
</div>

<a id="ys-node-test-cookies"></a>

## test_cookies

<div class="yarn-node" data-title="test_cookies">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: cookies</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Vuoi 5 biscotti?</span> <span class="yarn-meta">#line:0eb42ea </span>
<span class="yarn-line">SÌ</span> <span class="yarn-meta">#line:027a7a8 </span>
<span class="yarn-line">    Ecco qua! Buon divertimento!</span> <span class="yarn-meta">#line:0e5578d </span>
    <span class="yarn-cmd">&lt;&lt;cookies_add 5&gt;&gt;</span>
<span class="yarn-line">NO</span> <span class="yarn-meta">#line:0675243 </span>
<span class="yarn-line">    Ok, niente biscotti per te!</span> <span class="yarn-meta">#line:0662df0 </span>

&lt;&lt;if GetCookies() &gt;= 10&gt;&gt;
<span class="yarn-line">    Hai un sacco di biscotti!</span> <span class="yarn-meta">#line:0d2d1d7 </span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>


</code>
</pre>
</div>

<a id="ys-node-test-camera"></a>

## test_camera

<div class="yarn-node" data-title="test_camera">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: camera </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Adesso vi mostro la mappa del parco giochi</span> <span class="yarn-meta">#line:0c58bd8 </span>
<span class="yarn-cmd">&lt;&lt;camera_focus map_tutorial&gt;&gt;</span>
<span class="yarn-line">Capisci questo punto?</span> <span class="yarn-meta">#line:03e0224 </span>
<span class="yarn-cmd">&lt;&lt;camera_focus map_tutorial_detail&gt;&gt;</span>
<span class="yarn-line">Ora guarda quel palazzo!</span> <span class="yarn-meta">#line:0c1b2e4 </span>
<span class="yarn-cmd">&lt;&lt;camera_focus palace&gt;&gt;</span>
<span class="yarn-line">Bello, vero?</span> <span class="yarn-meta">#line:04daace </span>
<span class="yarn-cmd">&lt;&lt;camera_reset&gt;&gt;</span>


</code>
</pre>
</div>

<a id="ys-node-test-actions"></a>

## test_actions

<div class="yarn-node" data-title="test_actions">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags:</span>
<span class="yarn-header-dim">group: actions</span>
<span class="yarn-header-dim">actor: KID_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;action activate_elevator&gt;&gt;</span>
<span class="yarn-line">Azione Pre "pre_actions" e azione_post "post_actions"</span> <span class="yarn-meta">#line:0a67b25 </span>
<span class="yarn-line">Vuoi aprire il forziere?</span> <span class="yarn-meta">#line:03cae39 </span>
<span class="yarn-line">SÌ</span> <span class="yarn-meta">#line:067231b </span>
    <span class="yarn-cmd">&lt;&lt;action open_chest&gt;&gt;</span>
<span class="yarn-line">NO</span> <span class="yarn-meta">#line:05d9657 </span>
<span class="yarn-cmd">&lt;&lt;action stop_elevator&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-task-collect-apples"></a>

## task_collect_apples

<div class="yarn-node" data-title="task_collect_apples">
<pre class="yarn-code" style="--node-color:green"><code>
<span class="yarn-header-dim">tags:</span>
<span class="yarn-header-dim">group: tasks</span>
<span class="yarn-header-dim">color: green</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if HasCompletedTask("collect_apples")&gt;&gt;</span>
<span class="yarn-line">    Grazie! Hai completato questo compito</span> <span class="yarn-meta">#line:dev_task_collect_apples_2</span>
&lt;&lt;elseif GetCollectedItem("collect_apples") &gt; 0 &gt;&gt;
<span class="yarn-line">    Ho bisogno di più mele!</span> <span class="yarn-meta">#line:dev_task_collect_apples_3</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Raccogli 4 mele!</span> <span class="yarn-meta">#line:dev_task_collect_apples_1 </span>
    <span class="yarn-cmd">&lt;&lt;task_start collect_apples task_collect_apples_done&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-task-collect-apples-done"></a>

## task_collect_apples_done

<div class="yarn-node" data-title="task_collect_apples_done">
<pre class="yarn-code" style="--node-color:green"><code>
<span class="yarn-header-dim">tags:</span>
<span class="yarn-header-dim">group: tasks</span>
<span class="yarn-header-dim">color: green</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Ben fatto! Torna al robot</span> <span class="yarn-meta">#line:dev_test_task_done_1 </span>

</code>
</pre>
</div>

<a id="ys-node-task-open-chest"></a>

## task_open_chest

<div class="yarn-node" data-title="task_open_chest">
<pre class="yarn-code" style="--node-color:green"><code>
<span class="yarn-header-dim">tags:</span>
<span class="yarn-header-dim">group: tasks</span>
<span class="yarn-header-dim">color: green</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if HasCompletedTask("open_chest")&gt;&gt;</span>
<span class="yarn-line">    Grazie! Ne avevo bisogno</span> <span class="yarn-meta">#line:073d718 </span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Allunga la mano e apri quella cassa!</span> <span class="yarn-meta">#line:0c31fab </span>
    <span class="yarn-cmd">&lt;&lt;area off&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;target target_chest&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;task_start open_chest task_open_chest_done&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-task-open-chest-done"></a>

## task_open_chest_done

<div class="yarn-node" data-title="task_open_chest_done">
<pre class="yarn-code" style="--node-color:green"><code>
<span class="yarn-header-dim">tags:</span>
<span class="yarn-header-dim">group: tasks</span>
<span class="yarn-header-dim">color: green</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Ben fatto! Hai aperto il baule.</span> <span class="yarn-meta">#line:04e697a </span>
<span class="yarn-cmd">&lt;&lt;target off&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-item-chest"></a>

## item_chest

<div class="yarn-node" data-title="item_chest">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">tags:</span>
<span class="yarn-header-dim">group: actions</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">actor: NARRATOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Un bel baule!</span> <span class="yarn-meta">#line:dev_item_chest_1 </span>
<span class="yarn-cmd">&lt;&lt;action open_chest&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-npc-join-party"></a>

## npc_join_party

<div class="yarn-node" data-title="npc_join_party">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: party</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Vuoi unirti alla mia festa?</span> <span class="yarn-meta">#line:0b1f3e1 </span>
<span class="yarn-line">SÌ</span> <span class="yarn-meta">#line:0d1f4e3 </span>
    <span class="yarn-cmd">&lt;&lt;party_join npc_party_1&gt;&gt;</span>
<span class="yarn-line">    Ora ti seguo!</span> <span class="yarn-meta">#line:019353a </span>
<span class="yarn-line">NO</span> <span class="yarn-meta">#line:0a8e1c3 </span>
<span class="yarn-line">    OK</span> <span class="yarn-meta">#line:0c4e4ec </span>

</code>
</pre>
</div>

<a id="ys-node-npc-leave-party"></a>

## npc_leave_party

<div class="yarn-node" data-title="npc_leave_party">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: party</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Vuoi cambiare partito?</span> <span class="yarn-meta">#line:048e84e </span>
<span class="yarn-line">Cerchio di formazione</span> <span class="yarn-meta">#line:00c2f84 </span>
    <span class="yarn-cmd">&lt;&lt;party_formation circle &gt;&gt;</span>
<span class="yarn-line">Linea di formazione</span> <span class="yarn-meta">#line:03d5333 </span>
    <span class="yarn-cmd">&lt;&lt;party_formation line &gt;&gt;</span>
<span class="yarn-line">Formazione V</span> <span class="yarn-meta">#line:08ea78a </span>
    <span class="yarn-cmd">&lt;&lt;party_formation v &gt;&gt;</span>
<span class="yarn-line">Festa di rilascio</span> <span class="yarn-meta">#line:01f2e9a </span>
    <span class="yarn-cmd">&lt;&lt;party_release&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-test-quiz"></a>

## test_quiz

<div class="yarn-node" data-title="test_quiz">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: quiz</span>
<span class="yarn-header-dim">tags:</span>
<span class="yarn-header-dim">type:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Calcola 3*2</span> <span class="yarn-meta">#line:dev_test_quiz_1 </span>
<span class="yarn-line">quattro</span> <span class="yarn-meta">#line:dev_test_quiz_2 </span>
    <span class="yarn-cmd">&lt;&lt;jump test_quiz_wrong&gt;&gt;</span>
<span class="yarn-line">sei</span> <span class="yarn-meta">#line:dev_test_quiz_3 </span>
    <span class="yarn-cmd">&lt;&lt;jump test_quiz_correct&gt;&gt;</span>
<span class="yarn-line">otto</span> <span class="yarn-meta">#line:dev_test_quiz_4 </span>
    <span class="yarn-cmd">&lt;&lt;jump test_quiz_wrong&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-test-quiz-correct"></a>

## test_quiz_correct

<div class="yarn-node" data-title="test_quiz_correct">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: quiz</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">SÌ! Corretto.</span> <span class="yarn-meta">#line:0eb010e </span>

</code>
</pre>
</div>

<a id="ys-node-test-quiz-wrong"></a>

## test_quiz_wrong

<div class="yarn-node" data-title="test_quiz_wrong">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: quiz</span>

<span class="yarn-header-dim">---</span>
<span class="yarn-line">No. Vuoi riprovare?</span> <span class="yarn-meta">#line:0e2b8d7 </span>
<span class="yarn-line">SÌ</span> <span class="yarn-meta">#line:08ff2b9 </span>
    <span class="yarn-cmd">&lt;&lt;jump test_quiz&gt;&gt;</span>
<span class="yarn-line">NO</span> <span class="yarn-meta">#line:042881c </span>

</code>
</pre>
</div>

<a id="ys-node-endgame"></a>

## endgame

<div class="yarn-node" data-title="endgame">
<pre class="yarn-code" style="--node-color:red"><code>
<span class="yarn-header-dim">group: endgame</span>
<span class="yarn-header-dim">color: red</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Puoi terminare questa missione con...</span> <span class="yarn-meta">#line:027bdca </span>
<span class="yarn-line">NOOOO non voglio finire</span> <span class="yarn-meta">#line:014a238 </span>
<span class="yarn-line">SÌ, finiamola</span> <span class="yarn-meta">#line:0751111 </span>
    <span class="yarn-cmd">&lt;&lt;quest_end&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump close_game&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-close-game"></a>

## close_game

<div class="yarn-node" data-title="close_game">
<pre class="yarn-code" style="--node-color:red"><code>
<span class="yarn-header-dim">group: endgame</span>
<span class="yarn-header-dim">color: red</span>
<span class="yarn-header-dim">type: panel_endgame</span>
<span class="yarn-header-dim">---</span>

<span class="yarn-line">Questo è l'ultimo messaggio! arrivederci</span> <span class="yarn-meta">#line:01c70f4 </span>


</code>
</pre>
</div>

<a id="ys-node-npcgioconda"></a>

## NPCGIoconda

<div class="yarn-node" data-title="NPCGIoconda">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">assetimage: antura_hero</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;asset food_tomato&gt;&gt;</span>
<span class="yarn-line">Ciao, stefano</span> <span class="yarn-meta">#line:0c0329c </span>
<span class="yarn-line">ti piacciono le foto?</span> <span class="yarn-meta">#line:0b9b60d </span>
<span class="yarn-cmd">&lt;&lt;asset food_fish&gt;&gt;</span>
<span class="yarn-line">vuoi arancia?</span> <span class="yarn-meta">#line:0c2bd41 </span>
    <span class="yarn-cmd">&lt;&lt;asset food_orange&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;action play_sfx&gt;&gt;</span>
<span class="yarn-line">vuoi olio?</span> <span class="yarn-meta">#line:034ed48 </span>
    <span class="yarn-cmd">&lt;&lt;asset food_oil&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;action play_sfx&gt;&gt;</span>
<span class="yarn-line">ora chiuso</span> <span class="yarn-meta">#line:0baeb5f </span>
<span class="yarn-cmd">&lt;&lt;asset_hide&gt;&gt;</span>
<span class="yarn-line">Ordine di attività</span> <span class="yarn-meta">#line:0087ab1 </span>
    <span class="yarn-cmd">&lt;&lt;activity activity_test&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump activity_done&gt;&gt;</span>
<span class="yarn-line">Suona sfx</span> <span class="yarn-meta">#line:0f02889 </span>
    <span class="yarn-cmd">&lt;&lt;action play_sfx&gt;&gt;</span>
<span class="yarn-line">Apri Baule</span> <span class="yarn-meta">#line:053f800 </span>
    <span class="yarn-cmd">&lt;&lt;action open_chest&gt;&gt;</span>


</code>
</pre>
</div>

<a id="ys-node-activity-done"></a>

## activity_done

<div class="yarn-node" data-title="activity_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Attività terminata.</span> <span class="yarn-meta">#line:0ce187d </span>

</code>
</pre>
</div>

<a id="ys-node-test-setactive"></a>

## test_setactive

<div class="yarn-node" data-title="test_setactive">
<pre class="yarn-code"><code>

<span class="yarn-header-dim">---</span>
<span class="yarn-line">Ora attiverò gameObject test_setactive</span> <span class="yarn-meta">#line:0ef2850 </span>
<span class="yarn-cmd">&lt;&lt;SetActive TestSetActive_Crate&gt;&gt;</span>
<span class="yarn-line">E adesso lo spengo</span> <span class="yarn-meta">#line:0a360b1 </span>
<span class="yarn-cmd">&lt;&lt;SetActive TestSetActive_Crate false&gt;&gt;</span>
<span class="yarn-line">Bello, vero?</span> <span class="yarn-meta">#line:06bd992 </span>

</code>
</pre>
</div>

<a id="ys-node-npcgreeting2"></a>

## NPCGreeting2

<div class="yarn-node" data-title="NPCGreeting2">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">assetimage: pirates</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-comment">// &lt;&lt;activity activity_memory&gt;&gt;</span>
<span class="yarn-line">Ciao</span> <span class="yarn-meta">#line:08ecb6c </span>
<span class="yarn-line">Come stai?</span> <span class="yarn-meta">#line:00b7839 </span>
<span class="yarn-line">Bene</span> <span class="yarn-meta">#line:0991140 </span>
<span class="yarn-line">    Buono a sapersi</span> <span class="yarn-meta">#line:0b3477e </span>
<span class="yarn-line">Cattivo</span> <span class="yarn-meta">#line:08607a4 </span>
<span class="yarn-line">    Oh, mi dispiace sentirlo</span> <span class="yarn-meta">#line:0e6ecb2 </span>
<span class="yarn-line">Neutro</span> <span class="yarn-meta">#line:05f3ea5 </span>
<span class="yarn-line">    Capisco, spero che migliori presto</span> <span class="yarn-meta">#line:0802baf </span>
<span class="yarn-line">Cosa vuoi fare con le monete?</span> <span class="yarn-meta">#line:0bcd506 </span>

</code>
</pre>
</div>

<a id="ys-node-npcgreeting3"></a>

## NPCGreeting3

<div class="yarn-node" data-title="NPCGreeting3">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags:</span>
<span class="yarn-header-dim">---</span>

<span class="yarn-cmd">&lt;&lt;declare $coins = 0&gt;&gt;</span>
<span class="yarn-line">Cosa vuoi fare con {0} monete?</span> <span class="yarn-meta">#line:0a1994c </span>
<span class="yarn-line">Apri la porta</span> <span class="yarn-meta">#line:0e11bee </span>
 <span class="yarn-cmd">&lt;&lt;set $doorUnlocked = true&gt;&gt;</span>
<span class="yarn-line"> La porta è aperta</span> <span class="yarn-meta">#line:0cac01d </span>
<span class="yarn-line">Resta a casa</span> <span class="yarn-meta">#line:07f4239 </span>
<span class="yarn-line"> Tu resti a casa</span> <span class="yarn-meta">#line:079b16e </span>
 <span class="yarn-cmd">&lt;&lt;set $coins = $coins + 10&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-chest-01"></a>

## Chest_01

<div class="yarn-node" data-title="Chest_01">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $doorUnlocked&gt;&gt;</span>
<span class="yarn-line"> La porta è aperta</span> <span class="yarn-meta">#line:0e22a89 </span>
 <span class="yarn-cmd">&lt;&lt;jump EntroInCasa&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line"> La porta è chiusa</span> <span class="yarn-meta">#line:05086bc</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-note-for-the-editors"></a>

## note_for_the_editors

<div class="yarn-node" data-title="note_for_the_editors">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">style: note</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Nota personalizzata</span> <span class="yarn-meta">#line:04d621e </span>
<span class="yarn-line">questo è utile per lasciare note per gli editori</span> <span class="yarn-meta">#line:0520fb4 </span>
<span class="yarn-line">come promemoria di cose da fare</span> <span class="yarn-meta">#line:078fe99 </span>
<span class="yarn-line">o cose da controllare</span> <span class="yarn-meta">#line:0982259 </span>
<span class="yarn-line">o cose da sistemare</span> <span class="yarn-meta">#line:096379b </span>

</code>
</pre>
</div>

<a id="ys-node-entroincasa"></a>

## EntroInCasa

<div class="yarn-node" data-title="EntroInCasa">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">entro in casa</span> <span class="yarn-meta">#line:0f92df5 </span>

</code>
</pre>
</div>

<a id="ys-node-coin-machine"></a>

## coin_machine

<div class="yarn-node" data-title="coin_machine">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags:</span>
<span class="yarn-header-dim">group: variables</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Ti darò 100 monete.</span> <span class="yarn-meta">#line:06427f9 </span>
<span class="yarn-cmd">&lt;&lt;set $TOTAL_COINS = $TOTAL_COINS + 100&gt;&gt;</span>


</code>
</pre>
</div>

<a id="ys-node-test-mood"></a>

## test_mood

<div class="yarn-node" data-title="test_mood">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags: mood=HAPPY</span>
<span class="yarn-header-dim">actor: GUIDE_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Umore felice</span> <span class="yarn-meta">#line:0a6e648 </span>

</code>
</pre>
</div>

<a id="ys-node-random-lines"></a>

## random_lines

<div class="yarn-node" data-title="random_lines">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor: SENIOR_F</span>
<span class="yarn-header-dim">group: random</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Linea casuale intelligente 1</span> <span class="yarn-meta">#line:07193d8 </span>
<span class="yarn-line">Linea casuale intelligente 2</span> <span class="yarn-meta">#line:06fe9a7 </span>
<span class="yarn-line">Linea casuale intelligente 3</span> <span class="yarn-meta">#line:032e777 </span>

</code>
</pre>
</div>

<a id="ys-node-multiple-lines"></a>

## multiple_lines

<div class="yarn-node" data-title="multiple_lines">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: random</span>
<span class="yarn-header-dim">actor: KID_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Questo è il suggerimento 1</span> <span class="yarn-meta">#line:0423295 </span>
<span class="yarn-line">Questo è il suggerimento 2</span> <span class="yarn-meta">#line:00d69e5 </span>
<span class="yarn-line">Suggerimento 3</span> <span class="yarn-meta">#line:08966ba </span>

</code>
</pre>
</div>

<a id="ys-node-activity-canvas"></a>

## activity_canvas

<div class="yarn-node" data-title="activity_canvas">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">group: activities</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
&lt;&lt;if GetActivityResult("canvas_beach_settings") &gt; 0&gt;&gt;
    [MISSING TRANSLATION:     You already completed canvas 1!]
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>
<span class="yarn-line">Attività CANVAS!</span> <span class="yarn-meta">#line:0532f99 </span>
[MISSING TRANSLATION: -&gt; Yes #highlight]
    <span class="yarn-cmd">&lt;&lt;activity canvas_beach_settings activity_canvas_result&gt;&gt;</span>
[MISSING TRANSLATION: -&gt; No]


</code>
</pre>
</div>

<a id="ys-node-activity-canvas2"></a>

## activity_canvas2

<div class="yarn-node" data-title="activity_canvas2">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">group: activities</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
&lt;&lt;if GetActivityResult("canvas_beach2_settings") &gt; 0&gt;&gt;
    [MISSING TRANSLATION:     You already completed canvas 2!]
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>
[MISSING TRANSLATION: Do you want to play Activity CANVAS 2?]
[MISSING TRANSLATION: -&gt; Yes #highlight]
    <span class="yarn-cmd">&lt;&lt;activity canvas_beach2_settings activity_canvas2&gt;&gt;</span>
[MISSING TRANSLATION: -&gt; No]


</code>
</pre>
</div>

<a id="ys-node-activity-canvas-result"></a>

## activity_canvas_result

<div class="yarn-node" data-title="activity_canvas_result">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">group: activities</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
&lt;&lt;if GetActivityResult("") &gt; 0&gt;&gt;
<span class="yarn-line">Hai svolto bene l'attività CANVAS!</span> <span class="yarn-meta">#line:04fd2ce </span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">Riprova.</span> <span class="yarn-meta">#line:0797e1e </span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-activity-jigsaw"></a>

## activity_jigsaw

<div class="yarn-node" data-title="activity_jigsaw">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">group: activities</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Attività PUZZLE!</span> <span class="yarn-meta">#line:0d97ed9 </span>
<span class="yarn-cmd">&lt;&lt;activity jigsaw_toureiffel_settings activity_jigsaw_result&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-activity-jigsaw-result"></a>

## activity_jigsaw_result

<div class="yarn-node" data-title="activity_jigsaw_result">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">group: activities</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
&lt;&lt;if GetActivityResult("jigsaw_toureiffel_settings") &gt; 0&gt;&gt;
<span class="yarn-line">Hai giocato bene con l'attività PUZZLE!</span> <span class="yarn-meta">#line:041075b </span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">Riprova.</span> <span class="yarn-meta">#line:0921ca5 </span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-activity-memory"></a>

## activity_memory

<div class="yarn-node" data-title="activity_memory">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">group: activities</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
&lt;&lt;if GetActivityResult("memory_baguette_settings") &gt; 0&gt;&gt;
[MISSING TRANSLATION: You played MEMORY activity well!]
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>
<span class="yarn-line">Attività MEMORIA!</span> <span class="yarn-meta">#line:0727144</span>
<span class="yarn-cmd">&lt;&lt;activity memory_baguette_settings activity_memory_result expert&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-activity-memory-result"></a>

## activity_memory_result

<div class="yarn-node" data-title="activity_memory_result">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">group: activities</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
&lt;&lt;if GetActivityResult("memory_baguette_settings") &gt; 0&gt;&gt;
<span class="yarn-line">Hai svolto bene l'attività di MEMORIA!</span> <span class="yarn-meta">#line:03121d7 </span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">Riprova.</span> <span class="yarn-meta">#line:0d146ef </span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-activity-match"></a>

## activity_match

<div class="yarn-node" data-title="activity_match">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">group: activities</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Attività MATCH!</span> <span class="yarn-meta">#line:0949b00 </span>
<span class="yarn-cmd">&lt;&lt;activity match_zoo_1_settings activity_match_result&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-activity-match-result"></a>

## activity_match_result

<div class="yarn-node" data-title="activity_match_result">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">group: activities</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
&lt;&lt;if GetActivityResult("match_zoo_1_settings") &gt; 0&gt;&gt;
<span class="yarn-line">Hai giocato bene l'attività MATCH!</span> <span class="yarn-meta">#line:0426701 </span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">Riprova.</span> <span class="yarn-meta">#line:07643ad </span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-activity-money"></a>

## activity_money

<div class="yarn-node" data-title="activity_money">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">group: activities</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Attività SOLDI!</span> <span class="yarn-meta">#line:05b81d1 </span>
<span class="yarn-cmd">&lt;&lt;activity money_level_1_settings activity_money_result&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-activity-money-result"></a>

## activity_money_result

<div class="yarn-node" data-title="activity_money_result">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">group: activities</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
&lt;&lt;if GetActivityResult("money_level_1_settings") &gt; 0&gt;&gt;
<span class="yarn-line">Hai giocato bene all'attività MONEY!</span> <span class="yarn-meta">#line:0a63bb1 </span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">Riprova.</span> <span class="yarn-meta">#line:02b103d </span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-activity-order"></a>

## activity_order

<div class="yarn-node" data-title="activity_order">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">group: activities</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Attività ORDINE!</span> <span class="yarn-meta">#line:08e289f </span>
<span class="yarn-cmd">&lt;&lt;activity order_baguetterecipe_settings activity_order_result&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-activity-order-result"></a>

## activity_order_result

<div class="yarn-node" data-title="activity_order_result">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">group: activities</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
&lt;&lt;if GetActivityResult("order_baguetterecipe_settings") &gt; 0&gt;&gt;
<span class="yarn-line">Hai giocato bene l'attività ORDINA!</span> <span class="yarn-meta">#line:05527db </span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">Riprova.</span> <span class="yarn-meta">#line:04681e2 </span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-activity-piano"></a>

## activity_piano

<div class="yarn-node" data-title="activity_piano">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">group: activities</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Suona questa melodia per PIANOFORTE!</span> <span class="yarn-meta">#line:0e11b77 </span>
<span class="yarn-cmd">&lt;&lt;activity piano_framartino_settings activity_piano_result&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-activity-piano-result"></a>

## activity_piano_result

<div class="yarn-node" data-title="activity_piano_result">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">group: activities</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
&lt;&lt;if GetActivityResult("piano_framartino_settings") &gt; 0&gt;&gt;
<span class="yarn-line">Hai eseguito bene l'attività PIANOFORTE!</span> <span class="yarn-meta">#line:0b9f1af </span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">Riprova.</span> <span class="yarn-meta">#line:04c91cd </span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>


