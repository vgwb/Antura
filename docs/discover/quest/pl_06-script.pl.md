---
title: Pierniki i targ spożywczy (pl_06) - Script
hide:
---

# Pierniki i targ spożywczy (pl_06) - Script
[Quest Index](./index.pl.md) - Language: [english](./pl_06-script.md) - [french](./pl_06-script.fr.md) - polish - [italian](./pl_06-script.it.md)

!!! note "Educators & Designers: help improving this quest!"
    **Comments and feedback**: [discuss in the Forum](https://vgwb.discourse.group/t/pl-06-gingerbread-food-market/37/1)  
    **Improve translations**: [comment the Google Sheet](https://docs.google.com/spreadsheets/d/1FPFOy8CHor5ArSg57xMuPAG7WM27-ecDOiU-OmtHgjw/edit?gid=1211829352#gid=1211829352)  
    **Improve the script**: [propose an edit here](https://github.com/vgwb/Antura/blob/main/Assets/_discover/_quests/PL_06%20Torun%20Market/PL_06%20Torun%20Market%20-%20Yarn%20Script.yarn)  

<a id="ys-node-init"></a>
## init

<div class="yarn-node" data-title="init"><pre class="yarn-code" style="--node-color:red"><code><span class="yarn-header-dim">// PL_06_TORUN_MARKET - Gingerbread &amp; food market</span>
<span class="yarn-header-dim">// Location: Toruń, Poland - Medieval Market Square</span>
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
<span class="yarn-header-dim">// - buy pierogi for the cook</span>
<span class="yarn-header-dim">// - bake gingerbread (order/memory of ingredients)</span>
<span class="yarn-header-dim">// - Pierogi Challenge: order/memory of ingredients (flour, eggs, cheese, potatoes)</span>
<span class="yarn-header-dim">// Words used: Toruń, market, vendor, grocer, beekeeper, dairy, eggs, milk, butter, flour, honey, cloves, cinnamon, ginger, pierogi, molds, coins, zloty, kitchen, gingerbread, medieval</span>
<span class="yarn-header-dim">tags: type=Start</span>
<span class="yarn-header-dim">type: panel</span>
<span class="yarn-header-dim">color: red</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;set $TOTAL_COINS = 0&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $ingredients = 0&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $needed_ingredients = 8&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $pierogi_bought = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $got_eggs = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $got_flour = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $got_milk = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $got_butter = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $got_honey = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $got_cloves = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $got_cinnamon = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $got_ginger = false&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-intro"></a>
## intro

<div class="yarn-node" data-title="intro"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=GUIDE</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Antura is free but scared and hungry. <span class="yarn-meta">#line:0382fdb </span></span>
<span class="yarn-line">Let's go to the market in TORUŃ. <span class="yarn-meta">#line:03e11fa </span></span>
<span class="yarn-line">We will get food and ingredients. <span class="yarn-meta">#line:0522f6a </span></span>


</code></pre></div>

<a id="ys-node-market-hub"></a>
## market_hub

<div class="yarn-node" data-title="market_hub"><pre class="yarn-code"><code><span class="yarn-header-dim">group: market</span>
<span class="yarn-header-dim">tags: actor=GUIDE</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">GUIDE: Visit the stands and talk to vendors. <span class="yarn-meta">#line:0c470ae </span></span>
<span class="yarn-line">-&gt; Grocer (spices) <span class="yarn-meta">#line:0c33932 </span></span>
	<span class="yarn-cmd">&lt;&lt;camera_focus shop_grocer&gt;&gt;</span>
<span class="yarn-line">-&gt; Beekeeper (honey) <span class="yarn-meta">#line:0431cbc </span></span>
	<span class="yarn-cmd">&lt;&lt;camera_focus shop_beekeeper&gt;&gt;</span>
<span class="yarn-line">-&gt; Dairy vendor (milk, butter) <span class="yarn-meta">#line:0bfeb9e </span></span>
	<span class="yarn-cmd">&lt;&lt;camera_focus shop_dairy&gt;&gt;</span>
<span class="yarn-line">-&gt; Egg vendor (eggs) <span class="yarn-meta">#line:0aed436 </span></span>
	<span class="yarn-cmd">&lt;&lt;camera_focus shop_eggs&gt;&gt;</span>
<span class="yarn-line">-&gt; Greengrocer (fruits, vegetables) <span class="yarn-meta">#line:0b3cc4d </span></span>
	<span class="yarn-cmd">&lt;&lt;camera_focus shop_greengrocer&gt;&gt;</span>
<span class="yarn-line">-&gt; Cook (pierogi) <span class="yarn-meta">#line:0486ae8 </span></span>
	<span class="yarn-cmd">&lt;&lt;camera_focus shop_cook&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-shop-grocer"></a>
## shop_grocer

<div class="yarn-node" data-title="shop_grocer"><pre class="yarn-code"><code><span class="yarn-header-dim">group: market</span>
<span class="yarn-header-dim">tags: actor=OLD_WOMAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">I sell cloves, cinnamon, and ginger. I'm a grocer. <span class="yarn-meta">#line:0a66f1e </span></span>
<span class="yarn-line">-&gt; Buy cloves (1zł) <span class="yarn-meta">#line:0eff39a </span></span>
	<span class="yarn-cmd">&lt;&lt;jump pay_cloves&gt;&gt;</span>
<span class="yarn-line">-&gt; Buy cinnamon (1zł) <span class="yarn-meta">#line:02f52e1 </span></span>
	<span class="yarn-cmd">&lt;&lt;jump pay_cinnamon&gt;&gt;</span>
<span class="yarn-line">-&gt; Buy ginger (1zł) <span class="yarn-meta">#line:077537d </span></span>
	<span class="yarn-cmd">&lt;&lt;jump pay_ginger&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-shop-beekeeper"></a>
## shop_beekeeper

<div class="yarn-node" data-title="shop_beekeeper"><pre class="yarn-code"><code><span class="yarn-header-dim">group: market</span>
<span class="yarn-header-dim">tags: actor=MAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">I sell honey. I'm a beekeeper. <span class="yarn-meta">#line:001922d </span></span>
<span class="yarn-line">-&gt; Buy honey (1zł) <span class="yarn-meta">#line:086cd0b </span></span>
	<span class="yarn-cmd">&lt;&lt;jump pay_honey&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-shop-dairy"></a>
## shop_dairy

<div class="yarn-node" data-title="shop_dairy"><pre class="yarn-code"><code><span class="yarn-header-dim">group: market</span>
<span class="yarn-header-dim">tags: actor=WOMAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">I sell milk and butter. I'm a dairy vendor. <span class="yarn-meta">#line:0acb509 </span></span>
<span class="yarn-line">-&gt; Buy milk (5zł) <span class="yarn-meta">#line:0cd7285 </span></span>
	<span class="yarn-cmd">&lt;&lt;jump pay_milk&gt;&gt;</span>
<span class="yarn-line">-&gt; Buy butter (5zł) <span class="yarn-meta">#line:0e1775b </span></span>
	<span class="yarn-cmd">&lt;&lt;jump pay_butter&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-shop-eggs"></a>
## shop_eggs

<div class="yarn-node" data-title="shop_eggs"><pre class="yarn-code"><code><span class="yarn-header-dim">group: market</span>
<span class="yarn-header-dim">tags: actor=OLD_MAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">I sell eggs. I'm an egg vendor. <span class="yarn-meta">#line:03de236 </span></span>
<span class="yarn-line">-&gt; Buy eggs (10zł) <span class="yarn-meta">#line:07a79e8 </span></span>
	<span class="yarn-cmd">&lt;&lt;jump pay_eggs&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-shop-greengrocer"></a>
## shop_greengrocer

<div class="yarn-node" data-title="shop_greengrocer"><pre class="yarn-code"><code><span class="yarn-header-dim">group: market</span>
<span class="yarn-header-dim">tags: actor=WOMAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">I sell fruits and vegetables. I'm a greengrocer. <span class="yarn-meta">#line:082b55f </span></span>
<span class="yarn-line">Today you need spices and baking items. <span class="yarn-meta">#line:0af9f1e </span></span>
<span class="yarn-line">-&gt; Buy flour (2zł) <span class="yarn-meta">#line:0cf3d86 </span></span>
	<span class="yarn-cmd">&lt;&lt;jump pay_flour&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-shop-cook"></a>
## shop_cook

<div class="yarn-node" data-title="shop_cook"><pre class="yarn-code"><code><span class="yarn-header-dim">group: market</span>
<span class="yarn-header-dim">tags: actor=MAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">I sell pierogi. <span class="yarn-meta">#line:0d24718 </span></span>
<span class="yarn-line">If you buy pierogi for me, you can use my kitchen. <span class="yarn-meta">#line:0eda999 </span></span>
<span class="yarn-line">-&gt; Buy pierogi (20zł) <span class="yarn-meta">#line:01729c1 </span></span>
	<span class="yarn-cmd">&lt;&lt;jump pay_pierogi&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-pay-cloves"></a>
## pay_cloves

<div class="yarn-node" data-title="pay_cloves"><pre class="yarn-code"><code><span class="yarn-header-dim">group: pay</span>
<span class="yarn-header-dim">tags: actor=OLD_WOMAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Select enough money to pay 1zł. <span class="yarn-meta">#line:019a160 </span></span>
<span class="yarn-cmd">&lt;&lt;activity ACTIVITY_MONEY_1 add_cloves_done&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-add-cloves-done"></a>
## add_cloves_done

<div class="yarn-node" data-title="add_cloves_done"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: type=Variables</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if !$got_cloves&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;set $ingredients = $ingredients + 1&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;set $got_cloves = true&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>


</code></pre></div>

<a id="ys-node-pay-cinnamon"></a>
## pay_cinnamon

<div class="yarn-node" data-title="pay_cinnamon"><pre class="yarn-code"><code><span class="yarn-header-dim">group: pay</span>
<span class="yarn-header-dim">tags: actor=OLD_WOMAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Select enough money to pay 1zł. <span class="yarn-meta">#line:055af31 </span></span>
<span class="yarn-cmd">&lt;&lt;activity ACTIVITY_MONEY_1 add_cinnamon_done&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-add-cinnamon-done"></a>
## add_cinnamon_done

<div class="yarn-node" data-title="add_cinnamon_done"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: type=Variables</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if !$got_cinnamon&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;set $ingredients = $ingredients + 1&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;set $got_cinnamon = true&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>


</code></pre></div>

<a id="ys-node-pay-ginger"></a>
## pay_ginger

<div class="yarn-node" data-title="pay_ginger"><pre class="yarn-code"><code><span class="yarn-header-dim">group: pay</span>
<span class="yarn-header-dim">tags: actor=OLD_WOMAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Select enough money to pay 1zł. <span class="yarn-meta">#line:0172345 </span></span>
<span class="yarn-cmd">&lt;&lt;activity ACTIVITY_MONEY_1 add_ginger_done&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-add-ginger-done"></a>
## add_ginger_done

<div class="yarn-node" data-title="add_ginger_done"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: type=Variables</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if !$got_ginger&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;set $ingredients = $ingredients + 1&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;set $got_ginger = true&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>


</code></pre></div>

<a id="ys-node-pay-honey"></a>
## pay_honey

<div class="yarn-node" data-title="pay_honey"><pre class="yarn-code"><code><span class="yarn-header-dim">group: pay</span>
<span class="yarn-header-dim">tags: actor=MAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Select enough money to pay 1zł. <span class="yarn-meta">#line:0c1dcdb </span></span>
<span class="yarn-cmd">&lt;&lt;activity ACTIVITY_MONEY_1 add_honey_done&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-add-honey-done"></a>
## add_honey_done

<div class="yarn-node" data-title="add_honey_done"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: type=Variables</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if !$got_honey&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;set $ingredients = $ingredients + 1&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;set $got_honey = true&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-pay-milk"></a>
## pay_milk

<div class="yarn-node" data-title="pay_milk"><pre class="yarn-code"><code><span class="yarn-header-dim">group: pay</span>
<span class="yarn-header-dim">tags: actor=WOMAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Select enough money to pay 5zł. <span class="yarn-meta">#line:04a5053 </span></span>
<span class="yarn-cmd">&lt;&lt;activity ACTIVITY_MONEY_5 add_milk_done&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-add-milk-done"></a>
## add_milk_done

<div class="yarn-node" data-title="add_milk_done"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: type=Variables</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if !$got_milk&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;set $ingredients = $ingredients + 1&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;set $got_milk = true&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>


</code></pre></div>

<a id="ys-node-pay-butter"></a>
## pay_butter

<div class="yarn-node" data-title="pay_butter"><pre class="yarn-code"><code><span class="yarn-header-dim">group: pay</span>
<span class="yarn-header-dim">tags: actor=WOMAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Select enough money to pay 5zł. <span class="yarn-meta">#line:0bd010e </span></span>
<span class="yarn-cmd">&lt;&lt;activity ACTIVITY_MONEY_5 add_butter_done&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-add-butter-done"></a>
## add_butter_done

<div class="yarn-node" data-title="add_butter_done"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: type=Variables</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if !$got_butter&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;set $ingredients = $ingredients + 1&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;set $got_butter = true&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>


</code></pre></div>

<a id="ys-node-pay-eggs"></a>
## pay_eggs

<div class="yarn-node" data-title="pay_eggs"><pre class="yarn-code"><code><span class="yarn-header-dim">group: pay</span>
<span class="yarn-header-dim">tags: actor=OLD_MAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Select enough money to pay 10zł. <span class="yarn-meta">#line:0bdf451 </span></span>
<span class="yarn-cmd">&lt;&lt;activity ACTIVITY_MONEY_10 add_eggs_done&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-add-eggs-done"></a>
## add_eggs_done

<div class="yarn-node" data-title="add_eggs_done"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: type=Variables</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if !$got_eggs&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;set $ingredients = $ingredients + 1&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;set $got_eggs = true&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>


</code></pre></div>

<a id="ys-node-pay-flour"></a>
## pay_flour

<div class="yarn-node" data-title="pay_flour"><pre class="yarn-code"><code><span class="yarn-header-dim">group: pay</span>
<span class="yarn-header-dim">tags: actor=WOMAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Select enough money to pay 2zł. <span class="yarn-meta">#line:0aac231 </span></span>
<span class="yarn-cmd">&lt;&lt;activity ACTIVITY_MONEY_2 add_flour_done&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-add-flour-done"></a>
## add_flour_done

<div class="yarn-node" data-title="add_flour_done"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: type=Variables</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if !$got_flour&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;set $ingredients = $ingredients + 1&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;set $got_flour = true&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>


</code></pre></div>

<a id="ys-node-pay-pierogi"></a>
## pay_pierogi

<div class="yarn-node" data-title="pay_pierogi"><pre class="yarn-code"><code><span class="yarn-header-dim">group: pay</span>
<span class="yarn-header-dim">tags: actor=COOK</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;activity ACTIVITY_MONEY_20 pierogi_done&gt;&gt;</span>
<span class="yarn-line">Select enough money to pay 20zł. <span class="yarn-meta">#line:0dd43a0 </span></span>

</code></pre></div>

<a id="ys-node-pierogi-done"></a>
## pierogi_done

<div class="yarn-node" data-title="pierogi_done"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: type=Variables</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;set $pierogi_bought = true&gt;&gt;</span>


</code></pre></div>

<a id="ys-node-flour-hint"></a>
## flour_hint

<div class="yarn-node" data-title="flour_hint"><pre class="yarn-code"><code><span class="yarn-header-dim">group: market</span>
<span class="yarn-header-dim">tags: actor=WOMAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">You can buy FLOUR here. <span class="yarn-meta">#line:077f3e2 </span></span>
<span class="yarn-line">-&gt; Buy flour (2zł) <span class="yarn-meta">#line:00c5178 </span></span>
	<span class="yarn-cmd">&lt;&lt;jump pay_flour&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-go-to-kitchen"></a>
## go_to_kitchen

<div class="yarn-node" data-title="go_to_kitchen"><pre class="yarn-code"><code><span class="yarn-header-dim">group: kitchen</span>
<span class="yarn-header-dim">tags: actor=MAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Great! You have all the ingredients and pierogi for me. <span class="yarn-meta">#line:0dda4ac </span></span>
<span class="yarn-line">You can use my kitchen. <span class="yarn-meta">#line:019c938 </span></span>
<span class="yarn-line">Let's make Toruń gingerbread! <span class="yarn-meta">#line:0b5d503 </span></span>
<span class="yarn-cmd">&lt;&lt;jump bake_gingerbread&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-bake-gingerbread"></a>
## bake_gingerbread

<div class="yarn-node" data-title="bake_gingerbread"><pre class="yarn-code"><code><span class="yarn-header-dim">group: kitchen</span>
<span class="yarn-header-dim">tags: activity</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Match the gingerbread ingredients in the right order. <span class="yarn-meta">#line:0e54683 </span></span>
<span class="yarn-cmd">&lt;&lt;activity ACTIVITY_ORDER GINGERBREAD_ORDER_DONE&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-gingerbread-order-done"></a>
## GINGERBREAD_ORDER_DONE

<div class="yarn-node" data-title="GINGERBREAD_ORDER_DONE"><pre class="yarn-code"><code><span class="yarn-header-dim">group: kitchen</span>
<span class="yarn-header-dim">tags: actor=GUIDE</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Well done! The gingerbread is ready. <span class="yarn-meta">#line:0f082d6 </span></span>
<span class="yarn-line">Now, the Pierogi Challenge! <span class="yarn-meta">#line:0c0dc81 </span></span>
<span class="yarn-cmd">&lt;&lt;jump pierogi_challenge&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-pierogi-challenge"></a>
## pierogi_challenge

<div class="yarn-node" data-title="pierogi_challenge"><pre class="yarn-code"><code><span class="yarn-header-dim">group: kitchen</span>
<span class="yarn-header-dim">tags: activity</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Put the pierogi steps in order: flour, eggs, cheese, potatoes. <span class="yarn-meta">#line:01a6d63 </span></span>
<span class="yarn-cmd">&lt;&lt;activity ACTIVITY_ORDER PIEROGI_ORDER_DONE&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-pierogi-order-done"></a>
## PIEROGI_ORDER_DONE

<div class="yarn-node" data-title="PIEROGI_ORDER_DONE"><pre class="yarn-code"><code><span class="yarn-header-dim">group: kitchen</span>
<span class="yarn-header-dim">tags: actor=ANTURA</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Thank you! Gingerbread is a Toruń tradition. <span class="yarn-meta">#line:07b1ab4 </span></span>
<span class="yarn-line">I'm happy and full now. <span class="yarn-meta">#line:0f8e153 </span></span>
<span class="yarn-cmd">&lt;&lt;jump assessment_intro&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-assessment-intro"></a>
## assessment_intro

<div class="yarn-node" data-title="assessment_intro"><pre class="yarn-code"><code><span class="yarn-header-dim">// Assessment</span>
<span class="yarn-header-dim">group: assessment</span>
<span class="yarn-header-dim">tags: actor=GUIDE</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Time for a quick check. <span class="yarn-meta">#line:0bad7e0 </span></span>
<span class="yarn-cmd">&lt;&lt;jump assessment_vocab&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-assessment-vocab"></a>
## assessment_vocab

<div class="yarn-node" data-title="assessment_vocab"><pre class="yarn-code"><code><span class="yarn-header-dim">group: assessment</span>
<span class="yarn-header-dim">tags: activity</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Match words to pictures: eggs, flour, milk, bread, baker, grocer, honey, beekeeper, butcher, salt. <span class="yarn-meta">#line:0359266 </span></span>
<span class="yarn-cmd">&lt;&lt;activity ACTIVITY_ORDER VOCAB_MATCH_DONE&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-vocab-match-done"></a>
## VOCAB_MATCH_DONE

<div class="yarn-node" data-title="VOCAB_MATCH_DONE"><pre class="yarn-code"><code><span class="yarn-header-dim">group: assessment</span>
<span class="yarn-header-dim">tags: actor=GUIDE</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Great work! You helped Antura and learned new words. <span class="yarn-meta">#line:0cb84a7 </span></span>
<span class="yarn-cmd">&lt;&lt;quest_end 3&gt;&gt;</span>

</code></pre></div>


