/**
 * Yarn Graph Renderer - A library for rendering Yarn Spinner node graphs
 * Version: 1.0.0
 */

class YarnGraphRenderer {
    constructor(containerId, options = {}) {
        this.container = document.getElementById(containerId);
        if (!this.container) {
            throw new Error(`Container with id "${containerId}" not found`);
        }
        
        this.options = {
            width: options.width || '100%',
            height: options.height || 600,
            enableControls: options.enableControls !== false,
            enableTooltips: options.enableTooltips !== false,
            theme: options.theme || 'dark',
            ...options
        };
        
        // Ensure height is a number for calculations
        if (typeof this.options.height === 'string') {
            if (this.options.height.includes('vh')) {
                this.options.height = Math.round(window.innerHeight * parseFloat(this.options.height) / 100);
            } else if (this.options.height.includes('px')) {
                this.options.height = parseInt(this.options.height);
            } else if (this.options.height.includes('calc')) {
                // For calc values, use a reasonable default and let CSS handle it
                this.options.height = 600;
            } else {
                this.options.height = parseInt(this.options.height) || 600;
            }
        }
        
        this.nodes = new Map();
        this.edges = [];
        this.scale = 1;
        this.translateX = 0;
        this.translateY = 0;
        this.isDragging = false;
        this.lastMousePos = { x: 0, y: 0 };
        
        this.init();
    }
    
    init() {
        this.createDOM();
        this.setupEventHandlers();
        this.updateZoomDisplay();
    }
    
    createDOM() {
        this.container.className = 'yarn-graph-container';
        
        let controlsHTML = '';
        if (this.options.enableControls) {
            controlsHTML = `
                <div class="yarn-graph-controls">
                    <button onclick="window.yarnGraphInstances['${this.container.id}'].resetZoom()">Reset Zoom</button>
                    <button onclick="window.yarnGraphInstances['${this.container.id}'].fitToScreen()">Fit to Screen</button>
                    <button onclick="window.yarnGraphInstances['${this.container.id}'].centerAllNodes()">Force Center</button>
                    <button onclick="window.yarnGraphInstances['${this.container.id}'].toggleFullscreen()" class="fullscreen-btn">⛶ Fullscreen</button>
                    <button onclick="window.yarnGraphInstances['${this.container.id}'].exportSVG()">Export SVG</button>
                    <button onclick="window.yarnGraphInstances['${this.container.id}'].debugViewport()">Debug</button>
                    <div class="yarn-graph-zoom-info">
                        Zoom: <span class="zoom-level">100%</span>
                    </div>
                </div>
            `;
        }
        
        this.container.innerHTML = `
            ${controlsHTML}
            <div class="yarn-graph-viewport" style="height: ${this.options.height}px; min-height: ${Math.max(400, this.options.height)}px;">
                <svg class="yarn-graph-svg" width="100%" height="100%">
                    <defs>
                        <marker id="yarn-arrowhead-${this.container.id}" markerWidth="10" markerHeight="7" 
                                refX="9" refY="3.5" orient="auto" class="yarn-arrowhead">
                            <polygon points="0 0, 10 3.5, 0 7" fill="#888" />
                        </marker>
                    </defs>
                    <g class="yarn-graph-group"></g>
                </svg>
                <div class="yarn-graph-fullscreen-hint">Press ESC to exit fullscreen</div>
            </div>
        `;
        
        this.viewport = this.container.querySelector('.yarn-graph-viewport');
        this.svg = this.container.querySelector('.yarn-graph-svg');
        this.group = this.container.querySelector('.yarn-graph-group');
        this.zoomDisplay = this.container.querySelector('.zoom-level');
        this.fullscreenBtn = this.container.querySelector('.fullscreen-btn');
        this.isFullscreen = false;
        
        // Ensure viewport has proper dimensions
        console.log('📦 Viewport created:', {
            width: this.viewport.offsetWidth,
            height: this.viewport.offsetHeight,
            style: this.viewport.style.height
        });
        
        // Register instance globally for button callbacks
        if (!window.yarnGraphInstances) {
            window.yarnGraphInstances = {};
        }
        window.yarnGraphInstances[this.container.id] = this;
    }
    
    setupEventHandlers() {
        // Mouse events for panning - capture all mouse events on viewport
        this.viewport.addEventListener('mousedown', (e) => {
            // Always allow panning unless it's a control button
            if (!e.target.closest('button')) {
                this.isDragging = true;
                this.lastMousePos = { x: e.clientX, y: e.clientY };
                e.preventDefault();
                e.stopPropagation();
            }
        });
        
        document.addEventListener('mousemove', (e) => {
            if (this.isDragging) {
                const deltaX = e.clientX - this.lastMousePos.x;
                const deltaY = e.clientY - this.lastMousePos.y;
                this.translateX += deltaX / this.scale;
                this.translateY += deltaY / this.scale;
                this.updateTransform();
                this.lastMousePos = { x: e.clientX, y: e.clientY };
                e.preventDefault();
                e.stopPropagation();
            }
        });
        
        document.addEventListener('mouseup', (e) => {
            if (this.isDragging) {
                this.isDragging = false;
                e.preventDefault();
                e.stopPropagation();
            }
        });
        
        // Mouse wheel for zooming
        this.viewport.addEventListener('wheel', (e) => {
            e.preventDefault();
            const rect = this.viewport.getBoundingClientRect();
            const mouseX = e.clientX - rect.left;
            const mouseY = e.clientY - rect.top;
            
            const zoomFactor = e.deltaY > 0 ? 0.9 : 1.1;
            const newScale = Math.max(0.1, Math.min(5, this.scale * zoomFactor));
            
            if (newScale !== this.scale) {
                const scaleChange = newScale / this.scale;
                this.translateX = mouseX - (mouseX - this.translateX) * scaleChange;
                this.translateY = mouseY - (mouseY - this.translateY) * scaleChange;
                this.scale = newScale;
                this.updateTransform();
                this.updateZoomDisplay();
            }
        });
        
        // Touch events for mobile
        let touches = [];
        this.viewport.addEventListener('touchstart', (e) => {
            touches = Array.from(e.touches);
        });
        
        this.viewport.addEventListener('touchmove', (e) => {
            e.preventDefault();
            if (touches.length === 1 && e.touches.length === 1) {
                // Single touch - pan
                const deltaX = e.touches[0].clientX - touches[0].clientX;
                const deltaY = e.touches[0].clientY - touches[0].clientY;
                this.translateX += deltaX / this.scale;
                this.translateY += deltaY / this.scale;
                this.updateTransform();
            }
            touches = Array.from(e.touches);
        });
        
        // Keyboard shortcuts
        document.addEventListener('keydown', (e) => {
            if (e.target.closest('.yarn-graph-container') === this.container) {
                switch (e.key) {
                    case 'f':
                    case 'F':
                        if (e.ctrlKey || e.metaKey) {
                            e.preventDefault();
                            this.toggleFullscreen();
                        }
                        break;
                    case 'Escape':
                        if (this.isFullscreen) {
                            e.preventDefault();
                            this.exitFullscreen();
                        }
                        break;
                    case '0':
                        if (e.ctrlKey || e.metaKey) {
                            e.preventDefault();
                            this.resetZoom();
                        }
                        break;
                    case '1':
                        if (e.ctrlKey || e.metaKey) {
                            e.preventDefault();
                            this.fitToScreen();
                        }
                        break;
                }
            }
        });
    }
    
    updateTransform() {
        this.group.setAttribute('transform', 
            `translate(${this.translateX}, ${this.translateY}) scale(${this.scale})`);
    }
    
    updateZoomDisplay() {
        if (this.zoomDisplay) {
            this.zoomDisplay.textContent = Math.round(this.scale * 100) + '%';
        }
    }
    
    parseYarnScript(script) {
        console.log('🔍 Parsing Yarn script...');
        const nodes = new Map();
        
        // Split by === but keep the delimiter info
        const blocks = script.split('===').filter(block => block.trim());
        console.log(`📄 Found ${blocks.length} potential blocks`);
        
        blocks.forEach((block, blockIndex) => {
            console.log(`\n🔍 Processing block ${blockIndex + 1}:`);
            console.log('Block content (first 100 chars):', block.substring(0, 100).replace(/\n/g, '\\n'));
            
            const lines = block.trim().split('\n').map(line => line.trim()).filter(line => line);
            if (lines.length === 0) {
                console.log('  ⏭️ Empty block, skipping');
                return;
            }
            
            const node = { 
                connections: [], 
                choices: [],
                x: 0,
                y: 0,
                color: 'default'
            };
            
            let i = 0;
            let inHeader = true;
            
            // Parse header section
            while (i < lines.length && inHeader) {
                const line = lines[i];
                console.log(`  📋 Header line ${i}: "${line}"`);
                
                if (line === '---') {
                    console.log('  🔄 Found header delimiter, switching to body');
                    inHeader = false;
                    i++;
                    continue;
                }
                
                if (line.startsWith('title:')) {
                    node.title = line.replace('title:', '').trim();
                    console.log(`    📌 Title: "${node.title}"`);
                } else if (line.startsWith('position:')) {
                    const coords = line.replace('position:', '').trim().split(',');
                    if (coords.length >= 2) {
                        node.x = parseInt(coords[0]) + 500;
                        node.y = parseInt(coords[1]) + 500;
                        console.log(`    📍 Position: (${node.x}, ${node.y})`);
                    }
                } else if (line.startsWith('color:')) {
                    node.color = line.replace('color:', '').trim();
                    console.log(`    🎨 Color: ${node.color}`);
                } else if (line.startsWith('tags:')) {
                    const tags = line.replace('tags:', '').trim();
                    node.tags = tags;
                    if (tags.includes('type=Start')) {
                        node.isStart = true;
                        console.log('    🚀 Start node detected');
                    }
                    if (tags.includes('type=Choice')) {
                        node.isChoice = true;
                        console.log('    🔀 Choice node detected');
                    }
                } else if (line.startsWith('group:')) {
                    node.group = line.replace('group:', '').trim();
                    console.log(`    👥 Group: ${node.group}`);
                } else if (line.startsWith('actor:')) {
                    node.actor = line.replace('actor:', '').trim();
                    console.log(`    🎭 Actor: ${node.actor}`);
                }
                i++;
            }
            
            // Parse body content
            const bodyLines = [];
            while (i < lines.length) {
                const line = lines[i];
                console.log(`  📄 Body line ${i}: "${line}"`);
                
                if (line) {
                    // Look for connections
                    const jumpMatch = line.match(/<<jump\s+(.+?)>>/);
                    if (jumpMatch) {
                        const target = jumpMatch[1].trim();
                        node.connections.push(target);
                        console.log(`    🔗 Jump connection: -> ${target}`);
                    }
                    
                    const nextTargetMatch = line.match(/<<nexttarget\s+(.+?)>>/);
                    if (nextTargetMatch) {
                        const target = nextTargetMatch[1].trim();
                        node.connections.push(target);
                        console.log(`    🔗 Next target connection: -> ${target}`);
                    }
                    
                    const nextMatch = line.match(/<<next_target\s+(.+?)>>/);
                    if (nextMatch) {
                        const target = nextMatch[1].trim();
                        node.connections.push(target);
                        console.log(`    🔗 Next target connection: -> ${target}`);
                    }
                    
                    // Look for choices
                    if (line.startsWith('->')) {
                        const choiceMatch = line.match(/^->\s*(.+?):/);
                        if (choiceMatch) {
                            const choice = choiceMatch[1].trim();
                            node.choices.push(choice);
                            console.log(`    🔀 Choice: "${choice}"`);
                        }
                    }
                    
                    bodyLines.push(line);
                }
                i++;
            }
            
            node.content = bodyLines.join('\n');
            
            if (node.title) {
                console.log(`  ✅ Adding node: "${node.title}" at (${node.x}, ${node.y})`);
                nodes.set(node.title, node);
            } else {
                console.log(`  ❌ Skipping block ${blockIndex + 1}: no title found`);
            }
        });
        
        console.log(`✅ Parsing complete. Found ${nodes.size} nodes total.`);
        return nodes;
    }
    
    renderGraph(yarnScript) {
        console.log('🎯 Starting to render graph...');
        console.log('📝 Yarn script length:', yarnScript.length);
        console.log('📝 First 200 chars:', yarnScript.substring(0, 200));
        
        this.nodes = this.parseYarnScript(yarnScript);
        console.log('📊 Parsed nodes:', this.nodes.size);
        this.nodes.forEach((node, title) => {
            console.log(`  - ${title}:`, { x: node.x, y: node.y, connections: node.connections });
        });
        
        if (this.nodes.size === 0) {
            console.error('❌ No nodes parsed! Check your Yarn script format.');
            this.container.innerHTML = `
                <div style="padding: 20px; text-align: center; color: #ff6666; background: #2a2a2a;">
                    <h3>No nodes found!</h3>
                    <p>The Yarn script couldn't be parsed. Check the console for details.</p>
                    <details style="margin-top: 10px; text-align: left;">
                        <summary>Script preview (first 500 chars):</summary>
                        <pre style="background: #1a1a1a; padding: 10px; margin-top: 10px; overflow: auto; max-height: 200px;">${yarnScript.substring(0, 500)}</pre>
                    </details>
                </div>
            `;
            return;
        }
        
        this.edges = [];
        this.group.innerHTML = '';
        
        // Create edges based on connections
        this.nodes.forEach((node, title) => {
            node.connections.forEach(targetTitle => {
                if (this.nodes.has(targetTitle)) {
                    this.edges.push({
                        source: title,
                        target: targetTitle,
                        type: 'connection'
                    });
                    console.log(`🔗 Edge: ${title} -> ${targetTitle}`);
                } else {
                    console.warn(`⚠️ Connection target not found: ${title} -> ${targetTitle}`);
                }
            });
        });
        
        console.log('🔗 Total edges:', this.edges.length);
        
        // Render edges first (behind nodes)
        this.edges.forEach(edge => this.renderEdge(edge));
        
        // Render nodes
        this.nodes.forEach((node, title) => this.renderNode(node, title));
        
        console.log('✅ Rendering complete. Fitting to screen...');
        
        // Auto-fit after rendering
        setTimeout(() => {
            this.fitToScreen();
            console.log('📏 Fit to screen complete.');
        }, 100);
    }
    
    renderNode(node, title) {
        const group = document.createElementNS('http://www.w3.org/2000/svg', 'g');
        group.classList.add('yarn-node');
        
        // Apply node styling
        if (node.isStart) group.classList.add('start');
        else if (node.isChoice) group.classList.add('choice');
        else if (node.color) group.classList.add(node.color);
        else group.classList.add('default');
        
        const width = Math.max(140, Math.min(200, title.length * 9));
        const height = node.choices.length > 0 ? 80 : 60;
        
        // Background rectangle
        const rect = document.createElementNS('http://www.w3.org/2000/svg', 'rect');
        rect.setAttribute('x', node.x - width/2);
        rect.setAttribute('y', node.y - height/2);
        rect.setAttribute('width', width);
        rect.setAttribute('height', height);
        group.appendChild(rect);
        
        // Title text
        const titleText = document.createElementNS('http://www.w3.org/2000/svg', 'text');
        titleText.classList.add('yarn-node-title');
        titleText.setAttribute('x', node.x);
        titleText.setAttribute('y', node.y - (height > 60 ? 15 : 5));
        titleText.textContent = this.truncateText(title, 20);
        group.appendChild(titleText);
        
        // Subtitle (group/actor)
        if (node.group || node.actor) {
            const subText = document.createElementNS('http://www.w3.org/2000/svg', 'text');
            subText.classList.add('yarn-node-subtitle');
            subText.setAttribute('x', node.x);
            subText.setAttribute('y', node.y + (height > 60 ? 5 : 15));
            subText.textContent = this.truncateText(node.group || node.actor, 18);
            group.appendChild(subText);
        }
        
        // Choice indicator
        if (node.choices.length > 0) {
            const choiceText = document.createElementNS('http://www.w3.org/2000/svg', 'text');
            choiceText.classList.add('yarn-node-subtitle');
            choiceText.setAttribute('x', node.x);
            choiceText.setAttribute('y', node.y + 25);
            choiceText.textContent = `${node.choices.length} choice${node.choices.length > 1 ? 's' : ''}`;
            group.appendChild(choiceText);
        }
        
        // Add tooltip if enabled
        if (this.options.enableTooltips) {
            this.addTooltip(group, node, title);
        }
        
        this.group.appendChild(group);
    }
    
    renderEdge(edge) {
        const sourceNode = this.nodes.get(edge.source);
        const targetNode = this.nodes.get(edge.target);
        
        if (!sourceNode || !targetNode) return;
        
        // Calculate connection points on node edges
        const dx = targetNode.x - sourceNode.x;
        const dy = targetNode.y - sourceNode.y;
        const distance = Math.sqrt(dx * dx + dy * dy);
        
        if (distance === 0) return;
        
        const nodeRadius = 70; // Approximate node radius
        const startX = sourceNode.x + (dx / distance) * nodeRadius;
        const startY = sourceNode.y + (dy / distance) * nodeRadius;
        const endX = targetNode.x - (dx / distance) * nodeRadius;
        const endY = targetNode.y - (dy / distance) * nodeRadius;
        
        const path = document.createElementNS('http://www.w3.org/2000/svg', 'path');
        path.classList.add('yarn-edge');
        if (edge.type === 'choice') path.classList.add('choice');
        
        // Use curved path for better visualization
        const midX = (startX + endX) / 2;
        const midY = (startY + endY) / 2;
        const curvature = Math.min(50, distance * 0.2);
        
        const d = `M ${startX} ${startY} Q ${midX + curvature} ${midY} ${endX} ${endY}`;
        path.setAttribute('d', d);
        path.setAttribute('marker-end', `url(#yarn-arrowhead-${this.container.id})`);
        
        this.group.appendChild(path);
    }
    
    addTooltip(element, node, title) {
        let tooltip = null;
        let isShowingTooltip = false;
        
        element.addEventListener('mouseenter', (e) => {
            // Only show tooltip if not dragging
            if (!this.isDragging) {
                isShowingTooltip = true;
                tooltip = document.createElement('div');
                tooltip.classList.add('yarn-tooltip');
                
                let content = `<strong>${title}</strong>`;
                if (node.group) content += `\nGroup: ${node.group}`;
                if (node.actor) content += `\nActor: ${node.actor}`;
                if (node.choices.length > 0) {
                    content += `\n\nChoices:\n${node.choices.map(c => `• ${c}`).join('\n')}`;
                }
                if (node.content) {
                    const cleanContent = node.content
                        .replace(/<<.*?>>/g, '') // Remove Yarn commands
                        .replace(/\n+/g, '\n')   // Normalize line breaks
                        .trim();
                    if (cleanContent) {
                        content += `\n\n${cleanContent.substring(0, 200)}${cleanContent.length > 200 ? '...' : ''}`;
                    }
                }
                
                tooltip.textContent = content;
                document.body.appendChild(tooltip);
                
                const updateTooltipPosition = (e) => {
                    if (!tooltip || this.isDragging) return;
                    
                    const rect = tooltip.getBoundingClientRect();
                    let left = e.clientX + 10;
                    let top = e.clientY + 10;
                    
                    // Keep tooltip in viewport
                    if (left + rect.width > window.innerWidth) {
                        left = e.clientX - rect.width - 10;
                    }
                    if (top + rect.height > window.innerHeight) {
                        top = e.clientY - rect.height - 10;
                    }
                    
                    tooltip.style.left = left + 'px';
                    tooltip.style.top = top + 'px';
                };
                
                updateTooltipPosition(e);
                
                const mouseMoveHandler = updateTooltipPosition;
                const mouseLeaveHandler = () => {
                    isShowingTooltip = false;
                    if (tooltip) {
                        tooltip.remove();
                        tooltip = null;
                    }
                    element.removeEventListener('mousemove', mouseMoveHandler);
                    element.removeEventListener('mouseleave', mouseLeaveHandler);
                };
                
                element.addEventListener('mousemove', mouseMoveHandler);
                element.addEventListener('mouseleave', mouseLeaveHandler);
            }
        });
        
        // Hide tooltip when dragging starts
        element.addEventListener('mousedown', () => {
            if (tooltip && isShowingTooltip) {
                tooltip.remove();
                tooltip = null;
                isShowingTooltip = false;
            }
        });
    }
    
    truncateText(text, maxLength) {
        return text.length > maxLength ? text.substring(0, maxLength - 3) + '...' : text;
    }
    
    resetZoom() {
        this.scale = 1;
        this.translateX = 0;
        this.translateY = 0;
        this.updateTransform();
        this.updateZoomDisplay();
    }
    
    fitToScreen() {
        console.log('📏 Fitting to screen...');
        
        if (this.nodes.size === 0) {
            console.log('  ⚠️ No nodes to fit');
            return;
        }
        
        const bounds = this.getGraphBounds();
        const containerRect = this.viewport.getBoundingClientRect();
        
        console.log('  📊 Node bounds:', bounds);
        console.log('  📦 Container:', {
            width: containerRect.width,
            height: containerRect.height
        });
        
        if (bounds.width === 0 || bounds.height === 0) {
            console.log('  ⚠️ Invalid bounds, centering nodes');
            // Center the graph in the viewport
            this.scale = 1;
            this.translateX = containerRect.width / 2;
            this.translateY = containerRect.height / 2;
        } else {
            const padding = 100; // Increased padding
            const availableWidth = containerRect.width - padding * 2;
            const availableHeight = containerRect.height - padding * 2;
            
            const scaleX = availableWidth / bounds.width;
            const scaleY = availableHeight / bounds.height;
            this.scale = Math.min(scaleX, scaleY, 1); // Don't scale up beyond 100%
            
            // Center the scaled content
            const scaledWidth = bounds.width * this.scale;
            const scaledHeight = bounds.height * this.scale;
            
            this.translateX = (containerRect.width - scaledWidth) / 2 - bounds.minX * this.scale;
            this.translateY = (containerRect.height - scaledHeight) / 2 - bounds.minY * this.scale;
        }
        
        console.log('  📏 Final transform:', {
            scale: this.scale,
            translateX: this.translateX,
            translateY: this.translateY
        });
        
        this.updateTransform();
        this.updateZoomDisplay();
    }
    
    getGraphBounds() {
        let minX = Infinity, minY = Infinity, maxX = -Infinity, maxY = -Infinity;
        
        this.nodes.forEach(node => {
            const margin = 100;
            minX = Math.min(minX, node.x - margin);
            minY = Math.min(minY, node.y - margin);
            maxX = Math.max(maxX, node.x + margin);
            maxY = Math.max(maxY, node.y + margin);
        });
        
        return {
            minX, minY, maxX, maxY,
            width: maxX - minX,
            height: maxY - minY
        };
    }
    
    exportSVG() {
        const svgData = new XMLSerializer().serializeToString(this.svg);
        const blob = new Blob([svgData], { type: 'image/svg+xml' });
        const url = URL.createObjectURL(blob);
        
        const link = document.createElement('a');
        link.href = url;
        link.download = 'yarn-graph.svg';
        document.body.appendChild(link);
        link.click();
        document.body.removeChild(link);
        URL.revokeObjectURL(url);
    }
    
    // Force center all nodes for testing
    centerAllNodes() {
        if (this.nodes.size === 0) return;
        
        const containerRect = this.viewport.getBoundingClientRect();
        const centerX = containerRect.width / 2;
        const centerY = containerRect.height / 2;
        
        console.log('🎯 Centering all nodes at:', { centerX, centerY });
        
        // Reset transform
        this.scale = 1;
        this.translateX = 0;
        this.translateY = 0;
        
        // Re-render all nodes at center with small offsets
        this.group.innerHTML = '';
        
        let nodeIndex = 0;
        this.nodes.forEach((node, title) => {
            // Position nodes in a simple grid around center
            const offsetX = (nodeIndex % 3 - 1) * 150; // -150, 0, 150
            const offsetY = Math.floor(nodeIndex / 3) * 100; // 0, 100, 200, etc.
            
            // Temporarily override node position
            const originalX = node.x;
            const originalY = node.y;
            node.x = centerX + offsetX;
            node.y = centerY + offsetY;
            
            this.renderNode(node, title);
            
            // Restore original position
            node.x = originalX;
            node.y = originalY;
            
            nodeIndex++;
        });
        
        // Re-render edges with new positions
        this.edges.forEach(edge => this.renderEdge(edge));
        
        this.updateTransform();
    }
    
    // Debug method to check viewport and elements
    debugViewport() {
        console.log('🔧 DEBUG VIEWPORT:');
        console.log('- Container:', this.container.getBoundingClientRect());
        console.log('- Viewport:', this.viewport.getBoundingClientRect());
        console.log('- SVG:', this.svg.getBoundingClientRect());
        console.log('- Group children:', this.group.children.length);
        console.log('- Current transform:', this.group.getAttribute('transform'));
        console.log('- Scale:', this.scale);
        console.log('- Translate:', { x: this.translateX, y: this.translateY });
        console.log('- Fullscreen:', this.isFullscreen);
        
        // Check if nodes are actually there but invisible
        Array.from(this.group.children).forEach((child, i) => {
            if (child.tagName === 'g') {
                const rect = child.querySelector('rect');
                const text = child.querySelector('text');
                console.log(`- Node ${i}:`, {
                    class: child.className.baseVal,
                    rectPos: rect ? `${rect.getAttribute('x')},${rect.getAttribute('y')}` : 'no rect',
                    rectSize: rect ? `${rect.getAttribute('width')}x${rect.getAttribute('height')}` : 'no rect',
                    textContent: text ? text.textContent : 'no text',
                    visible: window.getComputedStyle(child).visibility !== 'hidden'
                });
            }
        });
        
        // Force show nodes at center if they exist but are positioned badly
        if (this.group.children.length > 0 && this.nodes.size > 0) {
            console.log('🎯 Forcing center position...');
            this.scale = 1;
            this.translateX = this.viewport.offsetWidth / 2;
            this.translateY = this.viewport.offsetHeight / 2;
            this.updateTransform();
        }
    }
    
    // Toggle fullscreen mode
    toggleFullscreen() {
        if (this.isFullscreen) {
            this.exitFullscreen();
        } else {
            this.enterFullscreen();
        }
    }
    
    // Enter fullscreen mode
    enterFullscreen() {
        console.log('🔍 Entering fullscreen mode...');
        this.isFullscreen = true;
        
        // Add fullscreen classes
        this.container.classList.add('fullscreen');
        document.body.classList.add('yarn-graph-fullscreen-active');
        
        // Update button text
        if (this.fullscreenBtn) {
            this.fullscreenBtn.textContent = '⊞ Exit Fullscreen';
            this.fullscreenBtn.classList.add('active');
        }
        
        // Refit to new dimensions after a short delay
        setTimeout(() => {
            this.fitToScreen();
        }, 100);
    }
    
    // Exit fullscreen mode
    exitFullscreen() {
        console.log('🔍 Exiting fullscreen mode...');
        this.isFullscreen = false;
        
        // Remove fullscreen classes
        this.container.classList.remove('fullscreen');
        document.body.classList.remove('yarn-graph-fullscreen-active');
        
        // Update button text
        if (this.fullscreenBtn) {
            this.fullscreenBtn.textContent = '⛶ Fullscreen';
            this.fullscreenBtn.classList.remove('active');
        }
        
        // Refit to new dimensions after a short delay
        setTimeout(() => {
            this.fitToScreen();
        }, 100);
    }
    
    // Static method to create and render a graph in one call
    static render(containerId, yarnScript, options = {}) {
        const renderer = new YarnGraphRenderer(containerId, options);
        renderer.renderGraph(yarnScript);
        return renderer;
    }
    
    // Static method to render from external file
    static renderFromFile(containerId, yarnFilePath, options = {}) {
        const renderer = new YarnGraphRenderer(containerId, options);
        return renderer.loadFromFile(yarnFilePath);
    }
    
    // Helper method to resolve asset paths in MkDocs
    static resolveMkDocsPath(relativePath) {
        // If the path starts with './' or doesn't start with '/', treat it as relative to current page
        if (relativePath.startsWith('./') || (!relativePath.startsWith('/') && !relativePath.startsWith('http'))) {
            // Get current page path
            const currentPath = window.location.pathname;
            const pathSegments = currentPath.split('/').filter(segment => segment);
            
            // Remove the current page from path if it's not a directory
            if (pathSegments.length > 0 && !currentPath.endsWith('/')) {
                pathSegments.pop();
            }
            
            // Handle relative path
            let cleanRelativePath = relativePath.replace(/^\.\//, ''); // Remove leading ./
            
            // Build the full path
            if (pathSegments.length > 0) {
                return `/${pathSegments.join('/')}/${cleanRelativePath}`;
            } else {
                return `/${cleanRelativePath}`;
            }
        }
        
        // Try to get base URL from canonical link (MkDocs adds this)
        const canonical = document.querySelector('link[rel="canonical"]');
        if (canonical) {
            const baseUrl = canonical.href.replace(/\/[^\/]*\/?$/, '');
            return `${baseUrl}/${relativePath}`;
        }
        
        // Fallback: calculate from current location
        const currentPath = window.location.pathname;
        const pathSegments = currentPath.split('/').filter(segment => segment);
        
        // Remove the current page from path
        if (pathSegments.length > 0 && !currentPath.endsWith('/')) {
            pathSegments.pop();
        }
        
        // Build relative path back to root
        const levelsUp = pathSegments.length;
        const relativePart = '../'.repeat(levelsUp);
        
        return relativePart + relativePath;
    }
    
    // Load and render from external file
    loadFromFile(yarnFilePath) {
        const resolvedPath = YarnGraphRenderer.resolveMkDocsPath(yarnFilePath);
        
        return fetch(resolvedPath)
            .then(response => {
                if (!response.ok) {
                    throw new Error(`Failed to load ${yarnFilePath}: ${response.status} ${response.statusText}`);
                }
                return response.text();
            })
            .then(yarnScript => {
                this.renderGraph(yarnScript);
                return this;
            })
            .catch(error => {
                console.error('Error loading Yarn file:', error);
                this.container.innerHTML = `
                    <div style="padding: 20px; text-align: center; color: #888;">
                        <p>Error loading story: ${yarnFilePath}</p>
                        <p><small>${error.message}</small></p>
                    </div>
                `;
                throw error;
            });
    }
}

// Export for use in modules
if (typeof module !== 'undefined' && module.exports) {
    module.exports = YarnGraphRenderer;
}

// Global registration
window.YarnGraphRenderer = YarnGraphRenderer;

// Debug helper function
window.debugYarnGraph = function(containerId) {
    const container = document.getElementById(containerId);
    if (!container) {
        console.error(`Container "${containerId}" not found`);
        return;
    }
    
    console.log('🔍 Container info:', {
        id: container.id,
        className: container.className,
        innerHTML: container.innerHTML.length + ' chars',
        children: container.children.length
    });
    
    const svg = container.querySelector('.yarn-graph-svg');
    const group = container.querySelector('.yarn-graph-group');
    
    if (svg) {
        console.log('📊 SVG info:', {
            width: svg.getAttribute('width'),
            height: svg.getAttribute('height'),
            children: svg.children.length
        });
    }
    
    if (group) {
        console.log('👥 Group info:', {
            transform: group.getAttribute('transform'),
            children: group.children.length,
            childElements: Array.from(group.children).map(child => ({
                tagName: child.tagName,
                className: child.className.baseVal || child.className
            }))
        });
    }
};