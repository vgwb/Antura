---
title: Pan di zenzero e mercato alimentare (pl_06) - Script
hide:
---

# Pan di zenzero e mercato alimentare (pl_06) - Script
> [!note] Educators & Designers: help improving this quest!
> **Comments and feedback**: [discuss in the Forum](https://antura.discourse.group/t/pl-06-gingerbread-food-market/37/1)  
> **Improve translations**: [comment the Google Sheet](https://docs.google.com/spreadsheets/d/1FPFOy8CHor5ArSg57xMuPAG7WM27-ecDOiU-OmtHgjw/edit?gid=1211829352#gid=1211829352)  
> **Improve the script**: [propose an edit here](https://github.com/vgwb/Antura/blob/main/Assets/_discover/_quests/PL_06%20Torun%20Market/PL_06%20Torun%20Market%20-%20Yarn%20Script.yarn)  

<a id="ys-node-quest-start"></a>

## quest_start

<div class="yarn-node" data-title="quest_start">
<pre class="yarn-code" style="--node-color:red"><code>
<span class="yarn-header-dim">// pl_06 | Market (Torun)</span>
<span class="yarn-header-dim">// </span>
<span class="yarn-header-dim">// ---------</span>
<span class="yarn-header-dim">// WANTED:</span>
<span class="yarn-header-dim">// Cards:</span>
<span class="yarn-header-dim">// - torun_gingerbread (cultural tradition)</span>
<span class="yarn-header-dim">// - torun_town_hall (Gothic architecture)</span>
<span class="yarn-header-dim">// - medieval_market (historical setting)</span>
<span class="yarn-header-dim">// - pierogi</span>
<span class="yarn-header-dim">// Tasks:</span>
<span class="yarn-header-dim">// - ingredient collection</span>
<span class="yarn-header-dim">// Activities:</span>
<span class="yarn-header-dim">// - money activities to pay</span>
<span class="yarn-header-dim">// - collect ingredients (eggs, flour, milk, butter, honey, cloves, cinnamon, ginger)</span>
<span class="yarn-header-dim">// - bake gingerbread (order/memory of ingredients)</span>
<span class="yarn-header-dim">// - Pierogi Challenge: order/memory of ingredients (flour, eggs, cheese, potatoes)</span>
<span class="yarn-header-dim">// Words used: Toruń, market, vendor, grocer, beekeeper, dairy, eggs, milk, butter, flour, honey, cloves, cinnamon, ginger, pierogi, molds, coins, zloty, kitchen, gingerbread, medieval</span>
<span class="yarn-header-dim">type: panel</span>
<span class="yarn-header-dim">color: red</span>
<span class="yarn-header-dim">actor: GUIDE_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;declare $got_cloves = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $got_cinnamon = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $got_ginger = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $got_honey = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $got_milk = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $got_butter = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $got_eggs = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $got_flour = false&gt;&gt;</span>

<span class="yarn-line">Benvenuti a TORUŃ!</span> <span class="yarn-meta">#line:080555e </span>
<span class="yarn-line">Andiamo al mercato di TORUŃ.</span> <span class="yarn-meta">#line:03e11fa </span>
<span class="yarn-cmd">&lt;&lt;target npc_cook&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-quest-end"></a>

## quest_end

<div class="yarn-node" data-title="quest_end">
<pre class="yarn-code" style="--node-color:green"><code>
<span class="yarn-header-dim">type: panel_endgame</span>
<span class="yarn-header-dim">color: green</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Ottimo lavoro al MARKET.</span> <span class="yarn-meta">#line:073978d </span>
<span class="yarn-line">Hai comprato e cucinato.</span> <span class="yarn-meta">#line:023d1f0 </span>
<span class="yarn-line">Pronti per un compito extra?</span> <span class="yarn-meta">#line:04e4583 </span>
<span class="yarn-cmd">&lt;&lt;jump post_quest_activity&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-post-quest-activity"></a>

## post_quest_activity

<div class="yarn-node" data-title="post_quest_activity">
<pre class="yarn-code" style="--node-color:green"><code>
<span class="yarn-header-dim">type: panel</span>
<span class="yarn-header-dim">color: green</span>
<span class="yarn-header-dim">tags: proposal</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Disegna una bancarella del MERCATO.</span> <span class="yarn-meta">#line:01f8217 </span>
<span class="yarn-line">Aggiungere UOVA FARINA LATTE BURRO.</span> <span class="yarn-meta">#line:0435fc9 </span>
<span class="yarn-line">Aggiungere MIELE, CHIODI DI GAROFANO, CANNELLA E ZENZERO.</span> <span class="yarn-meta">#line:0fed740 </span>
<span class="yarn-line">Scrivi 2 prezzi in zł.</span> <span class="yarn-meta">#line:084419e </span>
<span class="yarn-line">Mostralo a un amico.</span> <span class="yarn-meta">#line:0f16df6 </span>
<span class="yarn-cmd">&lt;&lt;quest_end&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-cook"></a>

## cook

<div class="yarn-node" data-title="cook">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: cook</span>
<span class="yarn-header-dim">actor: ADULT_M</span>
<span class="yarn-header-dim">---</span>
&lt;&lt;if GetCollectedItem("COLLECT_INGREDIENTS") &gt;= 8&gt;&gt;
    <span class="yarn-cmd">&lt;&lt;jump cook_ingredients_done&gt;&gt;</span>
&lt;&lt;elseif GetCollectedItem("COLLECT_INGREDIENTS") &gt; 0&gt;&gt;
    <span class="yarn-cmd">&lt;&lt;jump cook_not_enough&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump cook_welcome&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-cook-welcome"></a>

## cook_welcome

<div class="yarn-node" data-title="cook_welcome">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: cook</span>
<span class="yarn-header-dim">actor: ADULT_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">[MISSING TRANSLATION: Dzień dobry! I am the COOK.]</span> <span class="yarn-meta">#line:07fb019 </span>
<span class="yarn-line">[MISSING TRANSLATION: I want to make PIEROGI and GINGERBREAD.]</span> <span class="yarn-meta">#line:0201a0f </span>
<span class="yarn-line">[MISSING TRANSLATION: But I need INGREDIENTS.]</span> <span class="yarn-meta">#line:00a46f0 </span>
<span class="yarn-cmd">&lt;&lt;jump task_ingredients&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-task-ingredients"></a>

## task_ingredients

<div class="yarn-node" data-title="task_ingredients">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: cook</span>
<span class="yarn-header-dim">tags: task</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">[MISSING TRANSLATION: Go to the MARKET and buy INGREDIENTS.]</span> <span class="yarn-meta">#line:0c35a9c </span>
<span class="yarn-cmd">&lt;&lt;detour task_ingredients_desc&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;task_start COLLECT_INGREDIENTS task_ingredients_done&gt;&gt;</span>
<span class="yarn-line">[MISSING TRANSLATION: Remember to say "Dzień dobry" (Hello)]</span> <span class="yarn-meta">#line:091bb57 </span>
<span class="yarn-line">[MISSING TRANSLATION: and "Dziękuję" (Thank you).]</span> <span class="yarn-meta">#line:076ffac </span>
<span class="yarn-cmd">&lt;&lt;area area_full&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-task-ingredients-desc"></a>

## task_ingredients_desc

<div class="yarn-node" data-title="task_ingredients_desc">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">type: task</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">[MISSING TRANSLATION: Collect all the ingredients for the recipe. #task:COLLECT_INGREDIENTS]</span> <span class="yarn-meta">#line:00565e5 </span>
<span class="yarn-line">[MISSING TRANSLATION: CLOVES #card:cloves]</span> <span class="yarn-meta">#line:0feb774 </span>
<span class="yarn-line">[MISSING TRANSLATION: CINNAMON #card:cinnamon]</span> <span class="yarn-meta">#line:0ba5cca </span>
<span class="yarn-line">[MISSING TRANSLATION: GINGER #card:ginger]</span> <span class="yarn-meta">#line:0a700e3 </span>
<span class="yarn-line">[MISSING TRANSLATION: HONEY #card:honey]</span> <span class="yarn-meta">#line:0e22cab </span>
<span class="yarn-line">[MISSING TRANSLATION: MILK #card:milk]</span> <span class="yarn-meta">#line:08e580d </span>
<span class="yarn-line">[MISSING TRANSLATION: BUTTER #card:butter]</span> <span class="yarn-meta">#line:0bfb896 </span>
<span class="yarn-line">[MISSING TRANSLATION: EGGS #card:eggs]</span> <span class="yarn-meta">#line:0de2001 </span>
<span class="yarn-line">[MISSING TRANSLATION: FLOUR #card:flour]</span> <span class="yarn-meta">#line:070b733 </span>
<span class="yarn-cmd">&lt;&lt;card_hide&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-cook-not-enough"></a>

## cook_not_enough

<div class="yarn-node" data-title="cook_not_enough">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: cook</span>
<span class="yarn-header-dim">actor: ADULT_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">[MISSING TRANSLATION: You don't have all INGREDIENTS!]</span> <span class="yarn-meta">#line:003f24d </span>
<span class="yarn-line">[MISSING TRANSLATION: Visit the VENDORS in the MARKET.]</span> <span class="yarn-meta">#line:0d691b9 </span>

</code>
</pre>
</div>

<a id="ys-node-task-ingredients-done"></a>

## task_ingredients_done

<div class="yarn-node" data-title="task_ingredients_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: cook</span>
<span class="yarn-header-dim">type: panel</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;target npc_cook&gt;&gt;</span>
<span class="yarn-line">[MISSING TRANSLATION: You got all INGREDIENTS!]</span> <span class="yarn-meta">#line:03f7a4a </span>
<span class="yarn-line">[MISSING TRANSLATION: Go back to the COOK. #task:go_back_cook]</span> <span class="yarn-meta">#line:0222915 </span>
<span class="yarn-cmd">&lt;&lt;task_start go_back_cook&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-cook-ingredients-done"></a>

## cook_ingredients_done

<div class="yarn-node" data-title="cook_ingredients_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: cook</span>
<span class="yarn-header-dim">actor: ADULT_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">[MISSING TRANSLATION: Magnifique! You have everything.]</span> <span class="yarn-meta">#line:0bb1792 </span>
<span class="yarn-line">Prepariamo il PAN DI ZENZERO DI TORUŃ!</span> <span class="yarn-meta">#line:0b5d503</span>
<span class="yarn-cmd">&lt;&lt;jump bake_gingerbread&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-grocer"></a>

## grocer

<div class="yarn-node" data-title="grocer">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: grocer</span>
<span class="yarn-header-dim">actor: SENIOR_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $got_cloves and $got_cinnamon and $got_ginger&gt;&gt;</span>
<span class="yarn-line">    [MISSING TRANSLATION:     You bought all my SPICES!]</span> <span class="yarn-meta">#line:01e336a </span>
<span class="yarn-line">    [MISSING TRANSLATION:     -&gt; Dziękuję!]</span> <span class="yarn-meta">#line:thanks</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    [MISSING TRANSLATION:     -&gt; Dzień dobry!]</span> <span class="yarn-meta">#line:hello</span>
        <span class="yarn-cmd">&lt;&lt;jump grocer_hello&gt;&gt;</span>
    [MISSING TRANSLATION:     -&gt; Dziękuję! #shadow:thanks]
        <span class="yarn-cmd">&lt;&lt;jump talk_dont_understand&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-grocer-hello"></a>

## grocer_hello

<div class="yarn-node" data-title="grocer_hello">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: grocer</span>
<span class="yarn-header-dim">actor: SENIOR_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Vendo CHIODI DI GAROFANO, CANNELLA, ZENZERO. Sono un droghiere.</span> <span class="yarn-meta">#line:0a66f1e</span>
<span class="yarn-cmd">&lt;&lt;jump grocer_question&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-grocer-question"></a>

## grocer_question

<div class="yarn-node" data-title="grocer_question">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: grocer</span>
<span class="yarn-header-dim">actor: SENIOR_F</span>
<span class="yarn-header-dim">tags: type=Choice</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">[MISSING TRANSLATION: What do you want to BUY?]</span> <span class="yarn-meta">#line:06b8f99 </span>
<span class="yarn-cmd">&lt;&lt;if !$got_cloves&gt;&gt;</span>
<span class="yarn-line">Acquista chiodi di garofano (1zł)</span> <span class="yarn-meta">#line:0eff39a</span>
    <span class="yarn-cmd">&lt;&lt;jump pay_cloves&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;if !$got_cinnamon&gt;&gt;</span>
<span class="yarn-line">Acquista cannella (1zł)</span> <span class="yarn-meta">#line:02f52e1</span>
    <span class="yarn-cmd">&lt;&lt;jump pay_cinnamon&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;if !$got_ginger&gt;&gt;</span>
<span class="yarn-line">Acquista zenzero (1zł)</span> <span class="yarn-meta">#line:077537d</span>
    <span class="yarn-cmd">&lt;&lt;jump pay_ginger&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>
<span class="yarn-line">[MISSING TRANSLATION: -&gt; MILK and BUTTER]</span> <span class="yarn-meta">#line:wrong_dairy</span>
    <span class="yarn-cmd">&lt;&lt;jump talk_dont_sell&gt;&gt;</span>
<span class="yarn-line">[MISSING TRANSLATION: -&gt; EGGS]</span> <span class="yarn-meta">#line:wrong_eggs</span>
    <span class="yarn-cmd">&lt;&lt;jump talk_dont_sell&gt;&gt;</span>
<span class="yarn-line">[MISSING TRANSLATION: -&gt; Nothing]</span> <span class="yarn-meta">#line:new_nothing</span>

</code>
</pre>
</div>

<a id="ys-node-pay-cloves"></a>

## pay_cloves

<div class="yarn-node" data-title="pay_cloves">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: pay</span>
<span class="yarn-header-dim">actor: SENIOR_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card currency_zloty&gt;&gt;</span>
<span class="yarn-line">Seleziona le monete per pagare 1 zł.</span> <span class="yarn-meta">#line:019a160 </span>
<span class="yarn-cmd">&lt;&lt;activity ACTIVITY_MONEY_1 add_cloves_done&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-add-cloves-done"></a>

## add_cloves_done

<div class="yarn-node" data-title="add_cloves_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags: type=Variables</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;set $got_cloves = true&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;jump item_cloves&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-item-cloves"></a>

## item_cloves

<div class="yarn-node" data-title="item_cloves">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags: item</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card cloves&gt;&gt;</span>
<span class="yarn-line">[MISSING TRANSLATION: CLOVES]</span> <span class="yarn-meta">#line:item_cloves</span>
<span class="yarn-cmd">&lt;&lt;collect&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-pay-cinnamon"></a>

## pay_cinnamon

<div class="yarn-node" data-title="pay_cinnamon">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: pay</span>
<span class="yarn-header-dim">actor: SENIOR_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card coins&gt;&gt;</span>
<span class="yarn-line">Seleziona le monete per pagare 1 zł.</span> <span class="yarn-meta">#line:055af31 </span>
<span class="yarn-cmd">&lt;&lt;activity ACTIVITY_MONEY_1 add_cinnamon_done&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-add-cinnamon-done"></a>

## add_cinnamon_done

<div class="yarn-node" data-title="add_cinnamon_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags: type=Variables</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;set $got_cinnamon = true&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;jump item_cinnamon&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-item-cinnamon"></a>

## item_cinnamon

<div class="yarn-node" data-title="item_cinnamon">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags: item</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card cinnamon&gt;&gt;</span>
<span class="yarn-line">[MISSING TRANSLATION: CINNAMON]</span> <span class="yarn-meta">#line:item_cinnamon</span>
<span class="yarn-cmd">&lt;&lt;collect&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-pay-ginger"></a>

## pay_ginger

<div class="yarn-node" data-title="pay_ginger">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: pay</span>
<span class="yarn-header-dim">actor: SENIOR_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Seleziona le monete per pagare 1 zł.</span> <span class="yarn-meta">#line:0172345 </span>
<span class="yarn-cmd">&lt;&lt;activity ACTIVITY_MONEY_1 add_ginger_done&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-add-ginger-done"></a>

## add_ginger_done

<div class="yarn-node" data-title="add_ginger_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags: type=Variables</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;set $got_ginger = true&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;jump item_ginger&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-item-ginger"></a>

## item_ginger

<div class="yarn-node" data-title="item_ginger">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags: item</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card ginger&gt;&gt;</span>
<span class="yarn-line">[MISSING TRANSLATION: GINGER]</span> <span class="yarn-meta">#line:item_ginger</span>
<span class="yarn-cmd">&lt;&lt;collect&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-beekeeper"></a>

## beekeeper

<div class="yarn-node" data-title="beekeeper">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: beekeeper</span>
<span class="yarn-header-dim">actor: ADULT_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $got_honey&gt;&gt;</span>
<span class="yarn-line">    [MISSING TRANSLATION:     You bought my HONEY!]</span> <span class="yarn-meta">#line:new_honey_done</span>
    [MISSING TRANSLATION:     -&gt; Dziękuję! #shadow:thanks]
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
    [MISSING TRANSLATION:     -&gt; Dzień dobry! #shadow:hello]
        <span class="yarn-cmd">&lt;&lt;jump beekeeper_hello&gt;&gt;</span>
    [MISSING TRANSLATION:     -&gt; Dziękuję! #shadow:thanks]
        <span class="yarn-cmd">&lt;&lt;jump talk_dont_understand&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-beekeeper-hello"></a>

## beekeeper_hello

<div class="yarn-node" data-title="beekeeper_hello">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: beekeeper</span>
<span class="yarn-header-dim">actor: ADULT_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card beekeeper&gt;&gt;</span>
<span class="yarn-line">Vendo MIELE. Sono un apicoltore.</span> <span class="yarn-meta">#line:001922d </span>
<span class="yarn-line">Acquista miele (1zł)</span> <span class="yarn-meta">#line:086cd0b </span>
	<span class="yarn-cmd">&lt;&lt;jump pay_honey&gt;&gt;</span>
<span class="yarn-line">[MISSING TRANSLATION: -&gt; Buy FLOUR]</span> <span class="yarn-meta">#line:wrong_flour</span>
    <span class="yarn-cmd">&lt;&lt;jump talk_dont_sell&gt;&gt;</span>
<span class="yarn-line">[MISSING TRANSLATION: -&gt; Buy SPICES]</span> <span class="yarn-meta">#line:wrong_spices</span>
    <span class="yarn-cmd">&lt;&lt;jump talk_dont_sell&gt;&gt;</span>
<span class="yarn-line">[MISSING TRANSLATION: -&gt; No thanks]</span> <span class="yarn-meta">#line:new_no_bee</span>

</code>
</pre>
</div>

<a id="ys-node-pay-honey"></a>

## pay_honey

<div class="yarn-node" data-title="pay_honey">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: pay</span>
<span class="yarn-header-dim">actor: ADULT_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Seleziona le monete per pagare 1 zł.</span> <span class="yarn-meta">#line:0c1dcdb </span>
<span class="yarn-cmd">&lt;&lt;activity ACTIVITY_MONEY_1 add_honey_done&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-add-honey-done"></a>

## add_honey_done

<div class="yarn-node" data-title="add_honey_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags: type=Variables</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;set $got_honey = true&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;jump item_honey&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-item-honey"></a>

## item_honey

<div class="yarn-node" data-title="item_honey">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags: item</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card honey&gt;&gt;</span>
<span class="yarn-line">[MISSING TRANSLATION: HONEY]</span> <span class="yarn-meta">#line:item_honey</span>
<span class="yarn-cmd">&lt;&lt;collect&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-dairy"></a>

## dairy

<div class="yarn-node" data-title="dairy">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: dairy</span>
<span class="yarn-header-dim">actor: ADULT_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $got_milk and $got_butter&gt;&gt;</span>
<span class="yarn-line">    [MISSING TRANSLATION:     You bought MILK and BUTTER!]</span> <span class="yarn-meta">#line:new_dairy_done</span>
    [MISSING TRANSLATION:     -&gt; Dziękuję! #shadow:thanks]
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
    [MISSING TRANSLATION:     -&gt; Dzień dobry! #shadow:hello]
        <span class="yarn-cmd">&lt;&lt;jump dairy_hello&gt;&gt;</span>
    [MISSING TRANSLATION:     -&gt; Dziękuję! #shadow:thanks]
        <span class="yarn-cmd">&lt;&lt;jump talk_dont_understand&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-dairy-hello"></a>

## dairy_hello

<div class="yarn-node" data-title="dairy_hello">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: dairy</span>
<span class="yarn-header-dim">actor: ADULT_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card dairy_vendor&gt;&gt;</span>
<span class="yarn-line">Vendo LATTE e BURRO. Sono un venditore di latticini.</span> <span class="yarn-meta">#line:0acb509 </span>
<span class="yarn-cmd">&lt;&lt;jump dairy_question&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-dairy-question"></a>

## dairy_question

<div class="yarn-node" data-title="dairy_question">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: dairy</span>
<span class="yarn-header-dim">actor: ADULT_F</span>
<span class="yarn-header-dim">tags: type=Choice</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">[MISSING TRANSLATION: What do you want?]</span> <span class="yarn-meta">#line:new_dairy_q</span>
<span class="yarn-cmd">&lt;&lt;if !$got_milk&gt;&gt;</span>
<span class="yarn-line">Acquista il latte (5zł)</span> <span class="yarn-meta">#line:0cd7285 </span>
	<span class="yarn-cmd">&lt;&lt;jump pay_milk&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;if !$got_butter&gt;&gt;</span>
<span class="yarn-line">Acquista burro (5zł)</span> <span class="yarn-meta">#line:0e1775b </span>
	<span class="yarn-cmd">&lt;&lt;jump pay_butter&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>
<span class="yarn-line">[MISSING TRANSLATION: -&gt; SPICES]</span> <span class="yarn-meta">#line:wrong_spices_2</span>
    <span class="yarn-cmd">&lt;&lt;jump talk_dont_sell&gt;&gt;</span>
<span class="yarn-line">[MISSING TRANSLATION: -&gt; HONEY]</span> <span class="yarn-meta">#line:wrong_honey</span>
    <span class="yarn-cmd">&lt;&lt;jump talk_dont_sell&gt;&gt;</span>
<span class="yarn-line">[MISSING TRANSLATION: -&gt; Nothing]</span> <span class="yarn-meta">#line:new_nothing_dairy</span>

</code>
</pre>
</div>

<a id="ys-node-pay-milk"></a>

## pay_milk

<div class="yarn-node" data-title="pay_milk">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: pay</span>
<span class="yarn-header-dim">actor: ADULT_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Seleziona le monete per pagare 5 zł.</span> <span class="yarn-meta">#line:04a5053 </span>
<span class="yarn-cmd">&lt;&lt;activity ACTIVITY_MONEY_5 add_milk_done&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-add-milk-done"></a>

## add_milk_done

<div class="yarn-node" data-title="add_milk_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags: type=Variables</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;set $got_milk = true&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;jump item_milk&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-item-milk"></a>

## item_milk

<div class="yarn-node" data-title="item_milk">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags: item</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card milk&gt;&gt;</span>
<span class="yarn-line">[MISSING TRANSLATION: MILK]</span> <span class="yarn-meta">#line:item_milk</span>
<span class="yarn-cmd">&lt;&lt;collect&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-pay-butter"></a>

## pay_butter

<div class="yarn-node" data-title="pay_butter">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: pay</span>
<span class="yarn-header-dim">actor: ADULT_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Seleziona le monete per pagare 5 zł.</span> <span class="yarn-meta">#line:0bd010e </span>
<span class="yarn-cmd">&lt;&lt;activity ACTIVITY_MONEY_5 add_butter_done&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-add-butter-done"></a>

## add_butter_done

<div class="yarn-node" data-title="add_butter_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags: type=Variables</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;set $got_butter = true&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;jump item_butter&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-item-butter"></a>

## item_butter

<div class="yarn-node" data-title="item_butter">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags: item</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card butter&gt;&gt;</span>
<span class="yarn-line">[MISSING TRANSLATION: BUTTER]</span> <span class="yarn-meta">#line:item_butter</span>
<span class="yarn-cmd">&lt;&lt;collect&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-egg-vendor"></a>

## egg_vendor

<div class="yarn-node" data-title="egg_vendor">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: eggs</span>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $got_eggs&gt;&gt;</span>
<span class="yarn-line">    [MISSING TRANSLATION:     You bought EGGS!]</span> <span class="yarn-meta">#line:new_eggs_done</span>
    [MISSING TRANSLATION:     -&gt; Dziękuję! #shadow:thanks]
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
    [MISSING TRANSLATION:     -&gt; Dzień dobry! #shadow:hello]
        <span class="yarn-cmd">&lt;&lt;jump egg_hello&gt;&gt;</span>
    [MISSING TRANSLATION:     -&gt; Dziękuję! #shadow:thanks]
        <span class="yarn-cmd">&lt;&lt;jump talk_dont_understand&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-egg-hello"></a>

## egg_hello

<div class="yarn-node" data-title="egg_hello">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: eggs</span>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card egg_vendor&gt;&gt;</span>
<span class="yarn-line">Vendo UOVA. Sono un venditore ambulante di uova.</span> <span class="yarn-meta">#line:03de236 </span>
<span class="yarn-line">Acquista uova (10zł)</span> <span class="yarn-meta">#line:07a79e8 </span>
	<span class="yarn-cmd">&lt;&lt;jump pay_eggs&gt;&gt;</span>
<span class="yarn-line">[MISSING TRANSLATION: -&gt; Buy MILK]</span> <span class="yarn-meta">#line:wrong_milk</span>
    <span class="yarn-cmd">&lt;&lt;jump talk_dont_sell&gt;&gt;</span>
<span class="yarn-line">[MISSING TRANSLATION: -&gt; Buy GINGER]</span> <span class="yarn-meta">#line:wrong_ginger</span>
    <span class="yarn-cmd">&lt;&lt;jump talk_dont_sell&gt;&gt;</span>
<span class="yarn-line">[MISSING TRANSLATION: -&gt; No thanks]</span> <span class="yarn-meta">#line:new_no_eggs</span>

</code>
</pre>
</div>

<a id="ys-node-pay-eggs"></a>

## pay_eggs

<div class="yarn-node" data-title="pay_eggs">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: pay</span>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Seleziona le monete per pagare 10 zł.</span> <span class="yarn-meta">#line:0bdf451 </span>
<span class="yarn-cmd">&lt;&lt;activity ACTIVITY_MONEY_10 add_eggs_done&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-add-eggs-done"></a>

## add_eggs_done

<div class="yarn-node" data-title="add_eggs_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags: type=Variables</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;set $got_eggs = true&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;jump item_eggs&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-item-eggs"></a>

## item_eggs

<div class="yarn-node" data-title="item_eggs">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags: item</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card eggs&gt;&gt;</span>
<span class="yarn-line">[MISSING TRANSLATION: EGGS]</span> <span class="yarn-meta">#line:item_eggs</span>
<span class="yarn-cmd">&lt;&lt;collect&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-greengrocer"></a>

## greengrocer

<div class="yarn-node" data-title="greengrocer">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: greengrocer</span>
<span class="yarn-header-dim">actor: ADULT_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $got_flour&gt;&gt;</span>
<span class="yarn-line">    [MISSING TRANSLATION:     You bought FLOUR!]</span> <span class="yarn-meta">#line:new_flour_done</span>
    [MISSING TRANSLATION:     -&gt; Dziękuję! #shadow:thanks]
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
    [MISSING TRANSLATION:     -&gt; Dzień dobry! #shadow:hello]
        <span class="yarn-cmd">&lt;&lt;jump greengrocer_hello&gt;&gt;</span>
    [MISSING TRANSLATION:     -&gt; Dziękuję! #shadow:thanks]
        <span class="yarn-cmd">&lt;&lt;jump talk_dont_understand&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-greengrocer-hello"></a>

## greengrocer_hello

<div class="yarn-node" data-title="greengrocer_hello">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: greengrocer</span>
<span class="yarn-header-dim">actor: ADULT_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Vendo FRUTTA e VERDURA. Sono un fruttivendolo.</span> <span class="yarn-meta">#line:082b55f </span>
<span class="yarn-line">Oggi hai bisogno di SPEZIE e prodotti da forno.</span> <span class="yarn-meta">#line:0af9f1e </span>
<span class="yarn-line">Acquista la farina (2 zł)</span> <span class="yarn-meta">#line:0cf3d86 </span>
	<span class="yarn-cmd">&lt;&lt;jump pay_flour&gt;&gt;</span>
<span class="yarn-line">[MISSING TRANSLATION: -&gt; Buy SPICES]</span> <span class="yarn-meta">#line:wrong_spices_3</span>
    <span class="yarn-cmd">&lt;&lt;jump talk_dont_sell&gt;&gt;</span>
<span class="yarn-line">[MISSING TRANSLATION: -&gt; Buy EGGS]</span> <span class="yarn-meta">#line:wrong_eggs_2</span>
    <span class="yarn-cmd">&lt;&lt;jump talk_dont_sell&gt;&gt;</span>
<span class="yarn-line">[MISSING TRANSLATION: -&gt; No thanks]</span> <span class="yarn-meta">#line:new_no_flour</span>

</code>
</pre>
</div>

<a id="ys-node-pay-flour"></a>

## pay_flour

<div class="yarn-node" data-title="pay_flour">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: pay</span>
<span class="yarn-header-dim">actor: ADULT_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Seleziona le monete per pagare 2 zł.</span> <span class="yarn-meta">#line:0aac231 </span>
<span class="yarn-cmd">&lt;&lt;activity ACTIVITY_MONEY_2 add_flour_done&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-add-flour-done"></a>

## add_flour_done

<div class="yarn-node" data-title="add_flour_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags: type=Variables</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;set $got_flour = true&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;jump item_flour&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-item-flour"></a>

## item_flour

<div class="yarn-node" data-title="item_flour">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags: item</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card flour&gt;&gt;</span>
<span class="yarn-line">[MISSING TRANSLATION: FLOUR]</span> <span class="yarn-meta">#line:item_flour</span>
<span class="yarn-cmd">&lt;&lt;collect&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-bake-gingerbread"></a>

## bake_gingerbread

<div class="yarn-node" data-title="bake_gingerbread">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: kitchen</span>
<span class="yarn-header-dim">tags: activity</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card gingerbread_mold&gt;&gt;</span>
<span class="yarn-line">Abbina le parti del PAN DI ZENZERO in ordine.</span> <span class="yarn-meta">#line:0e54683 </span>
<span class="yarn-cmd">&lt;&lt;activity ACTIVITY_ORDER GINGERBREAD_ORDER_DONE&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-gingerbread-order-done"></a>

## GINGERBREAD_ORDER_DONE

<div class="yarn-node" data-title="GINGERBREAD_ORDER_DONE">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: kitchen</span>
<span class="yarn-header-dim">actor: GUIDE_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card gingerbread&gt;&gt;</span>
<span class="yarn-line">Ottimo lavoro! Il PAN DI ZENZERO è pronto.</span> <span class="yarn-meta">#line:0f082d6 </span>
<span class="yarn-line">Adesso la sfida dei PIEROGI!</span> <span class="yarn-meta">#line:0c0dc81 </span>
<span class="yarn-cmd">&lt;&lt;jump pierogi_challenge&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-pierogi-challenge"></a>

## pierogi_challenge

<div class="yarn-node" data-title="pierogi_challenge">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: kitchen</span>
<span class="yarn-header-dim">tags: activity</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Fasi dell'ordine: FARINA UOVA FORMAGGIO PATATE.</span> <span class="yarn-meta">#line:01a6d63 </span>
<span class="yarn-cmd">&lt;&lt;activity ACTIVITY_ORDER PIEROGI_ORDER_DONE&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-pierogi-order-done"></a>

## PIEROGI_ORDER_DONE

<div class="yarn-node" data-title="PIEROGI_ORDER_DONE">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: kitchen</span>
<span class="yarn-header-dim">actor: SPECIAL</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Grazie! È un cibo TORUŃ.</span> <span class="yarn-meta">#line:07b1ab4 </span>
<span class="yarn-line">Ora sono felice e sazio.</span> <span class="yarn-meta">#line:0f8e153 </span>
<span class="yarn-cmd">&lt;&lt;jump assessment_intro&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-assessment-intro"></a>

## assessment_intro

<div class="yarn-node" data-title="assessment_intro">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">// Assessment</span>
<span class="yarn-header-dim">group: assessment</span>
<span class="yarn-header-dim">actor: GUIDE_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">È il momento di un rapido controllo.</span> <span class="yarn-meta">#line:0bad7e0 </span>
<span class="yarn-cmd">&lt;&lt;jump assessment_vocab&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-assessment-vocab"></a>

## assessment_vocab

<div class="yarn-node" data-title="assessment_vocab">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: assessment</span>
<span class="yarn-header-dim">tags: activity</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Abbina le parole alle immagini</span> <span class="yarn-meta">#line:04fee1b </span>
<span class="yarn-cmd">&lt;&lt;activity ACTIVITY_ORDER VOCAB_MATCH_DONE&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-vocab-match-done"></a>

## VOCAB_MATCH_DONE

<div class="yarn-node" data-title="VOCAB_MATCH_DONE">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: assessment</span>
<span class="yarn-header-dim">actor: GUIDE_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Ottimo lavoro! Hai aiutato Antura a imparare le parole.</span> <span class="yarn-meta">#line:0cb84a7 </span>
<span class="yarn-cmd">&lt;&lt;jump quest_end&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-talk-dont-understand"></a>

## talk_dont_understand

<div class="yarn-node" data-title="talk_dont_understand">
<pre class="yarn-code" style="--node-color:orange"><code>
<span class="yarn-header-dim">tags: detour</span>
<span class="yarn-header-dim">color: orange</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">[MISSING TRANSLATION: =&gt; Sorry, I don't understand...]</span> <span class="yarn-meta">#line:dont_understand</span>

</code>
</pre>
</div>

<a id="ys-node-talk-dont-sell"></a>

## talk_dont_sell

<div class="yarn-node" data-title="talk_dont_sell">
<pre class="yarn-code" style="--node-color:orange"><code>
<span class="yarn-header-dim">tags: detour</span>
<span class="yarn-header-dim">color: orange</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">[MISSING TRANSLATION: =&gt; Sorry, I don't sell that.]</span> <span class="yarn-meta">#line:dont_sell</span>

</code>
</pre>
</div>

<a id="ys-node-spawned-child"></a>

## spawned_child

<div class="yarn-node" data-title="spawned_child">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">///////// NPCs SPAWNED IN THE SCENE //////////</span>
<span class="yarn-header-dim">// these npc are spawn automatically in the scene</span>
<span class="yarn-header-dim">// use these to add random facts. everythime you meet them</span>
<span class="yarn-header-dim">// they will say one of these lines randomly</span>
<span class="yarn-header-dim">group: Spawned</span>
<span class="yarn-header-dim">actor: KID_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Mi piace il PAN DI ZENZERO dolce.</span> <span class="yarn-meta">#line:087d4b0 </span>
<span class="yarn-line">Le monete tintinnano nella mia borsa.</span> <span class="yarn-meta">#line:0090fb8 </span>

</code>
</pre>
</div>

<a id="ys-node-spawned-vendor"></a>

## spawned_vendor

<div class="yarn-node" data-title="spawned_vendor">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: Spawned</span>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Oggi UOVA e LATTE freschi.</span> <span class="yarn-meta">#line:039e272 </span>
<span class="yarn-line">I pierogi si vendono molto velocemente.</span> <span class="yarn-meta">#line:07b9a99 </span>

</code>
</pre>
</div>

<a id="ys-node-spawned-tourist"></a>

## spawned_tourist

<div class="yarn-node" data-title="spawned_tourist">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: Spawned</span>
<span class="yarn-header-dim">actor: ADULT_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">La vecchia HALL sembra così alta.</span> <span class="yarn-meta">#line:041418b </span>
<span class="yarn-line">Assaggerò i PIEROGI.</span> <span class="yarn-meta">#line:068a9a7 </span>

</code>
</pre>
</div>


