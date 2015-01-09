/*
*
* Bluesky Components MusicPlayer Library v1.0
* 
* Copyright 2014 Yihuan Hu
*
*/
(function(Bluesky) {
    if (Bluesky && Bluesky.component) {
        Bluesky.extend(false, Bluesky.component, { MusicPlayer: function() {
            var args = arguments[0];
            return Bluesky.extend(true, {}, this, args);
        }
        });

        Bluesky.extend(false, Bluesky.component.MusicPlayer.prototype, {
            renderTo: "",
            id: "",
            width: 390,
            height: 441,
            list: [],
            activeIndex: 1,
            position: { x: 50, y: 50 },
            nodes: {
                wrapper: null,
                top: null,
                list: null,
                image: null,
                operaterarea: null,
                titlearea: null,

                previous: null,
                next: null,
                play: null,
                player: null
            },
            init: function() {
                this.nodes.wrapper = Bluesky.create("div", { className: "bluesky-musicplayer-wrapper" }).width(this.width).height(this.height).css("left", this.position.x + "px").css("top", this.position.y + "px");
                //this.nodes.wrapper.append(this.nodes.player = Bluesky.create("embed", { type: "audio/mp3", hidden: "true", loop: "false", width: "0", height: "0", MASTERSOUND: "", src: "SystemUpload/MyMusic/1/Song Of The Lonely Mountain.mp3" }));

                //this.nodes.wrapper.html("<embed type='audio' hidden='true' loop='false' width='0' height='0' MASTERSOUND src='SystemUpload/MyMusic/1/Song Of The Lonely Mountain.mp3'></embed>");
                var musicid = "MusicPlayer" + this.id;
                var playerHtml = '<div style="width:0px;height:0px;position:absolute;left:0px;top:0px;">' +
                                 '\n<object id="' + musicid + '" classid="clsid:D27CDB6E-AE6D-11cf-96B8-444553540000" codebase="http://fpdownload.macromedia.com/get/flashplayer/current/swflash.cab" width="0" height="0">' +
				                 '\n<param name="movie" value="Include/html/MusicPlay.swf" />' +
				                 '\n<param name="quality" value="high" />' +
				                 '\n<param name="allowScriptAccess" value="always" />' +
				                 '\n<embed name="' + musicid + '" allowScriptAccess="always" src="Include/html/MusicPlay.swf" quality="high" pluginspage="http://www.macromedia.com/go/getflashplayer" type="application/x-shockwave-flash" width="0" height="0"></embed>' +
				                 '\n</object>' +
				                 '\n</div>';
                this.nodes.wrapper.html(playerHtml);
                this.nodes.top = Bluesky.create("div", { className: "bluesky-musicplayer-top" });
                this.nodes.list = Bluesky.create("div", { className: "bluesky-musicplayer-list" }).height(this.height - 160);
                this.nodes.image = Bluesky.create("div", { className: "bluesky-musicplayer-image" });
                this.nodes.operaterarea = Bluesky.create("div", { className: "bluesky-musicplayer-operaterarea" });
                this.nodes.titlearea = Bluesky.create("div", { className: "bluesky-musicplayer-titlearea", html: "<font class='bluesky-musicplayer-title'>沧海医一声笑</font><br /><font class='bluesky-musicplayer-singer'>陈楚生</font>" });
                var processbar = Bluesky.create("div", { className: "bluesky-musicplayer-programbar" });
                var bottons = Bluesky.create("div", { className: "bluesky-musicplayer-buttons" });

                this.nodes.previous = Bluesky.create("a", { className: "bluesky-musicplayer-previous" });
                this.nodes.next = Bluesky.create("a", { className: "bluesky-musicplayer-next" });
                var closure = this;
                this.nodes.play = Bluesky.create("a", { className: "bluesky-musicplayer-play" }).addEvent("click", function() { closure.togglePlay(); });

                bottons.append(this.nodes.previous).append(this.nodes.play).append(this.nodes.next);

                this.nodes.top.append(this.nodes.image).append(this.nodes.operaterarea.append(this.nodes.titlearea).append(processbar).append(bottons));
                this.nodes.wrapper.append(this.nodes.top).append(this.nodes.list);
                Bluesky(this.renderTo).append(this.nodes.wrapper);
                this.nodes.player = Bluesky("#" + musicid).element();
                return this;
            },
            setPosition: function(_position) {
                if (_position) {
                    this.position = _position;
                }
                this.nodes.wrapper.css("left", this.position.x + "px").css("top", this.position.y + "px");
            },
            togglePlay: function() {
                this.nodes.play.hasClass("bluesky-musicplayer-play") ? this.play() : this.pause();
            },

            play: function(url) {
                this.nodes.play.replaceClass("bluesky-musicplayer-play", "bluesky-musicplayer-pause");
                this.nodes.player.jsPlay("SystemUpload/MyMusic/1/Song Of The Lonely Mountain.mp3");
            },
            stop: function() {
                this.nodes.player.jsStop();
            },
            pause: function() {
                this.nodes.play.replaceClass("bluesky-musicplayer-pause", "bluesky-musicplayer-play");
                this.nodes.player.jsPause();
            },
            rePlay: function() {
                this.nodes.player.jsRePlay();
            },
            jumpPlay: function(pos) {
                this.nodes.player.jsJumpPlay(pos);
            },
            setVolume: function(val) {
                this.nodes.player.jsSetVolume(val);
            },

            previous: function() {

            },

            next: function() {

            },
            show: function() {
                this.nodes.wrapper.css("left", this.position.x + "px");
            },
            hide: function() {
                this.nodes.wrapper.css("left", "10000px");
            },
            isHidden: function() {
                return this.nodes.wrapper.css("left") == "10000px";
            }

        });
    }
})(Bluesky);