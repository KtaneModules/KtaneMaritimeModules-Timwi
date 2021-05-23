﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MaritimeFlags;
using UnityEngine;
using Rnd = UnityEngine.Random;

/// <summary>
/// On the Subject of Maritime Flags
/// Created by Timwi
/// </summary>
public class MaritimeFlagsModule : MaritimeBase
{
    protected override string Name { get { return "Maritime Flags"; } }

    public Sprite[] Solves;
    public SpriteRenderer FlagDisplay1;
    public SpriteRenderer FlagDisplay2;
    public KMSelectable Compass;
    public Transform CompassNeedle;

    private Callsign _callsign;
    private int _bearingOnModule;
    private int _compassSolution;
    private Sprite[] _flagsOnModule;
    private int _currentFlagIndex;
    private int _curCompass;
    private Coroutine _submit;

    private static readonly Callsign[] _seed1Callsigns = @"1STMATE=355;2NDMATE=109;3RDMATE=250;ABANDON=308;ADMIRAL=260;ADVANCE=356;AGROUND=236;ALLIDES=28;ANCHORS=346;ATHWART=78;AZIMUTH=265;BAILERS=357;BALLAST=129;BARRACK=23;BEACHED=170;BEACONS=121;BEAMEND=259;BEAMSEA=316;BEARING=12;BEATING=105;BELAYED=297;BERMUDA=107;BOBSTAY=76;BOILERS=190;BOLLARD=258;BONNETS=332;BOOMKIN=29;BOUNDER=99;BOWLINE=73;BRAILED=165;BREADTH=293;BRIDGES=43;BRIGGED=191;BRINGTO=279;BULWARK=202;BUMBOAT=193;BUMPKIN=119;BURTHEN=294;CABOOSE=10;CAPSIZE=194;CAPSTAN=141;CAPTAIN=1;CARAVEL=295;CAREENS=217;CARRACK=241;CARRIER=94;CATBOAT=219;CATHEAD=177;CHAINED=162;CHANNEL=214;CHARLEY=123;CHARTER=228;CITADEL=246;CLEARED=306;CLEATED=290;CLINKER=163;CLIPPER=130;COAMING=198;COASTED=138;CONSORT=324;CONVOYS=301;CORINTH=225;COTCHEL=140;COUNTER=20;CRANZES=229;CREWING=27;CRINGLE=42;CROJACK=13;CRUISER=333;CUTTERS=147;DANDIES=309;DEADRUN=327;DEBUNKS=331;DERRICK=289;DIPPING=134;DISRATE=158;DOGVANE=231;DOLDRUM=238;DOLPHIN=139;DRAUGHT=351;DRIFTER=201;DROGUES=143;DRYDOCK=4;DUNNAGE=137;DUNSELS=287;EARINGS=60;ECHELON=350;EMBAYED=62;ENSIGNS=98;ESCORTS=11;FAIRWAY=86;FALKUSA=243;FANTAIL=178;FARDAGE=133;FATHOMS=49;FENDERS=337;FERRIES=358;FITTING=89;FLANKED=68;FLARING=172;FLATTOP=195;FLEMISH=323;FLOATED=187;FLOORED=262;FLOTSAM=67;FOLDING=15;FOLLOWS=148;FORCING=118;FORWARD=239;FOULIES=145;FOUNDER=330;FRAMING=156;FREIGHT=174;FRIGATE=227;FUNNELS=230;FURLING=204;GALLEON=300;GALLEYS=292;GALLIOT=9;GANGWAY=210;GARBLED=237;GENERAL=354;GEORGES=100;GHOSTED=30;GINPOLE=233;GIVEWAY=74;GONDOLA=85;GRAVING=253;GRIPIES=335;GROUNDS=153;GROWLER=281;GUINEAS=164;GUNDECK=82;GUNPORT=221;GUNWALE=36;HALYARD=325;HAMMOCK=261;HAMPERS=161;HANGARS=343;HARBORS=182;HARBOUR=154;HAULING=65;HAWSERS=51;HEADING=205;HEADSEA=52;HEAVING=298;HERRING=44;HOGGING=189;HOLIDAY=87;HUFFLER=282;INBOARD=53;INIRONS=111;INSHORE=91;INSTAYS=285;INWATER=103;INWAYOF=69;JACKIES=126;JACKTAR=110;JENNIES=61;JETTIES=71;JIGGERS=353;JOGGLES=215;JOLLIES=188;JURYRIG=166;KEELSON=66;KELLETS=41;KICKING=92;KILLICK=277;KITCHEN=235;LANYARD=155;LAYDAYS=122;LAZARET=283;LEEHELM=197;LEESIDE=116;LEEWARD=95;LIBERTY=81;LIGHTER=302;LIZARDS=276;LOADING=169;LOCKERS=180;LOFTING=186;LOLLING=34;LOOKOUT=171;LUBBERS=340;LUFFING=2;LUGGERS=106;LUGSAIL=284;MAEWEST=5;MANOWAR=348;MARCONI=244;MARINER=209;MATELOT=157;MIZZENS=47;MOORING=310;MOUSING=6;NARROWS=97;NIPPERS=179;OFFICER=252;OFFPIER=266;OILSKIN=31;OLDSALT=347;ONBOARD=249;OREBOAT=271;OUTHAUL=77;OUTWARD=125;PAINTER=75;PANTING=254;PARCELS=113;PARLEYS=38;PARRELS=334;PASSAGE=311;PELAGIC=159;PENDANT=274;PENNANT=135;PICKETS=339;PINNACE=149;PINTLES=317;PIRATES=37;PIVOTED=70;PURSERS=108;PURSUED=90;QUARTER=183;QUAYING=257;RABBETS=315;RATLINE=269;REDUCED=329;REEFERS=146;REPAIRS=35;RIGGING=14;RIPRAPS=167;ROMPERS=17;ROWLOCK=26;RUDDERS=286;RUFFLES=39;RUMMAGE=115;SAGGING=33;SAILORS=207;SALTIES=93;SALVORS=345;SAMPANS=79;SAMPSON=319;SCULLED=291;SCUPPER=206;SCUTTLE=58;SEACOCK=117;SEALING=22;SEEKERS=245;SERVING=220;SEXTANT=275;SHELTER=25;SHIPPED=299;SHIPRIG=247;SICKBAY=142;SKIPPER=45;SKYSAIL=84;SLINGED=196;SLIPWAY=83;SNAGGED=50;SNOTTER=199;SPLICED=268;SPLICES=181;SPONSON=19;SPONSOR=234;SPRINGS=273;SQUARES=263;STACKIE=132;STANDON=226;STARTER=59;STATION=359;STEAMER=57;STEERED=213;STEEVES=314;STEWARD=3;STOPPER=242;STOVEIN=185;STOWAGE=203;STRIKES=124;SUNFISH=342;SWIMMIE=307;SYSTEMS=255;TACKING=18;THWARTS=54;TINCLAD=63;TOMPION=270;TONNAGE=175;TOPMAST=305;TOPSAIL=338;TORPEDO=267;TOSSERS=114;TRADING=251;TRAFFIC=102;TRAMPER=150;TRANSOM=313;TRAWLER=127;TRENAIL=21;TRENNEL=173;TRIMMER=322;TROOPER=46;TRUNNEL=349;TUGBOAT=131;TURNTWO=303;UNSHIPS=318;UPBOUND=326;VESSELS=55;VOICING=321;VOYAGER=212;WEATHER=278;WHALERS=7;WHARVES=341;WHELKIE=223;WHISTLE=151;WINCHES=211;WINDAGE=222;WORKING=218;YARDARM=101"
        .Split(';').Select(str => str.Split('=')).Select(arr => new Callsign { Name = arr[0], Bearing = int.Parse(arr[1]) }).ToArray();
    private static readonly string[] _allCallsigns = @"1STMATE;2NDMATE;3RDMATE;ABANDON;ABOXLAW;ABREAST;ADDENDA;ADJUNCT;ADMIRAL;ADVANCE;ADVECTS;AERODYN;AFTRIGS;AGROUND;AIRTANK;ALADDIN;ALLHAIL;ALLIDES;ALLISON;ALMANAC;ALOWCUT;AMMETER;ANCHORS;ANEMONE;ANGLERS;APOSTLE;APPENDS;APPOINT;AQUATIC;ARTICLE;ATHRUST;ATHWART;ATTACKS;AVASTYE;AVOCADO;AWNINGS;AZIMUTH;BABOONS;BACKING;BAILERS;BALANCE;BALLAST;BARQUES;BARRACK;BARSHOT;BARTAUT;BATTLED;BATTLES;BEACHED;BEACONS;BEAMEND;BEAMSEA;BEARING;BEATING;BECALMS;BECKETS;BELAYED;BENEATH;BERMUDA;BETWIXT;BEWPARS;BEWPERS;BILBOES;BINGING;BISCUIT;BLACKEN;BLANKET;BLOOPER;BLOWING;BLUESEA;BOBSTAY;BOILERS;BOLLARD;BOLSTER;BONNETS;BOOMKIN;BOOTTOP;BOTTOMS;BOUNDER;BOWBEAM;BOWLINE;BOXDECK;BOXMARK;BRACING;BRAILED;BREADTH;BREAKER;BRIDGES;BRIGGED;BRINGTO;BRISTOL;BULKEND;BULWARK;BUMBOAT;BUMPERS;BUMPKIN;BUNKERS;BUNTING;BUOYANT;BURDENS;BURTHEN;BUTTOCK;BYBOARD;BYWINDS;CABOOSE;CALKING;CALVING;CAMBERS;CANBUOY;CAPSIZE;CAPSTAN;CAPTAIN;CARAVEL;CAREENS;CARGOES;CARLINE;CARLING;CARLINS;CARRACK;CARRICK;CARRIER;CATBOAT;CATHEAD;CATSKIN;CATSPAW;CATWALK;CELESTE;CERTIFY;CHAFING;CHAINED;CHANNEL;CHARLEY;CHARLIE;CHARTER;CHEARLY;CHEESED;CHIEFLY;CICADAS;CIRCLED;CIRCLES;CITADEL;CLEANED;CLEARED;CLEATED;CLINKER;CLIPPER;CLOSEST;CLOTHES;CLOVERS;COACHED;COACHES;COAMING;COASTAL;COASTED;COCKPIT;COLLIER;COLORED;COLREGS;COMBINE;COMPANY;CONSORT;CONTAIN;CONVENT;CONVOYS;COPPERS;CORDAGE;CORINTH;CORSAIR;COTCHEL;COUNTER;COVERED;CRACKON;CRADLED;CRADLES;CRAFTED;CRANKED;CRANZES;CREEPER;CRESTED;CREWING;CREWMAN;CREWMEN;CRIBBED;CRIMSON;CRINGLE;CROJACK;CROWNED;CRUISED;CRUISER;CUNNING;CURRENT;CUSTOMS;CUTTERS;CUTTING;DAGGERS;DANDIES;DAYMARK;DEADRUN;DEBARKS;DEBUNKS;DECKLOG;DECLINE;DELAYED;DEPARTS;DERRICK;DEVIATE;DINGBAT;DIPPING;DISABLE;DISMAST;DISRATE;DIURNAL;DIVIDER;DOCKING;DODGERS;DOGVANE;DOLDRUM;DOLPHIN;DONKEYS;DOORING;DORADES;DOUBLED;DOUBLES;DOUSERS;DRAFTED;DRAUGHT;DRAWING;DRIFTER;DRIVING;DROGUES;DRYDOCK;DUNNAGE;DUNSELS;EARINGS;EARRING;EASIEST;EASTERN;EBBTIDE;ECHELON;EFFORTS;EMBARGO;EMBARKS;EMBAYED;ENGINES;ENSIGNS;ENTRIES;EQUATOR;EQUINOX;ERRORED;ESCORTS;EYEBOLT;FAIRWAY;FALKUSA;FALLING;FANTAIL;FARDAGE;FASHION;FASTENS;FASTICE;FATHOMS;FCCRULE;FEATHER;FENDERS;FERRIED;FERRIES;FETCHED;FETCHES;FEVERED;FIDDLED;FIDDLES;FIDDLEY;FIGKNOT;FILLING;FINGERS;FIREMAN;FISHERY;FITTING;FIXMAST;FLAGGED;FLAKING;FLANKED;FLARING;FLASHED;FLASHES;FLATTEN;FLATTOP;FLEMISH;FLOATED;FLOORED;FLOTSAM;FLOWERS;FLUSHED;FLYBOAT;FOGHORN;FOLDING;FOLLOWS;FORCING;FOREAFT;FOREBIT;FOREGUY;FORWARD;FOULIES;FOUNDER;FOXTROT;FRACTAL;FRAMING;FRAZILS;FREEING;FREIGHT;FRESHEN;FRIGATE;FRONTED;FULLSEA;FUNNELS;FURLING;FUTTOCK;FUTZING;GADGETS;GAFFRIG;GAFFTOP;GALLEON;GALLERY;GALLEYS;GALLIOT;GAMMONS;GANGWAY;GARBLED;GARNETS;GARTERS;GASKETS;GEARING;GELCOAT;GENERAL;GENNIES;GEORGES;GHOSTED;GIMBALS;GINEBED;GINGERS;GINPOLE;GIRDLES;GIVEWAY;GMTTIME;GOABOUT;GOALOFT;GOBLINE;GOINGTO;GONDOLA;GOSSIPS;GOUNDER;GRAMPUS;GRAPPLE;GRAVING;GRIPIES;GROGGED;GROMMET;GROUNDS;GROWLER;GUDGEON;GUINEAS;GUNDECK;GUNNELS;GUNPORT;GUNWALE;GUSSETS;HALFSEA;HALYARD;HAMMOCK;HAMPERS;HANGARS;HANGING;HARBORS;HARBOUR;HARDENS;HATCHES;HAULERS;HAULING;HAULOUT;HAWSERS;HAWSING;HAZARDS;HEADERS;HEADING;HEADSEA;HEADSUP;HEADWAY;HEAVEIN;HEAVETO;HEAVING;HEELING;HERRING;HIGHSEA;HITCHED;HITCHES;HOGGING;HOISTED;HOLDING;HOLIDAY;HORIZON;HORSING;HOUSING;HUFFLER;HULLING;ICEBERG;INBOARD;INDEXES;INDICES;INDULGE;INFLATE;INIRONS;INLANDS;INSHORE;INSPECT;INSTAYS;INTEROP;INVERTS;INWATER;INWAYOF;ISOBARS;ISOBATH;ISOGONY;JACKIES;JACKTAR;JENNIES;JERQUES;JETSAMS;JETTIES;JIBBERS;JIBSTAY;JIGGERS;JOGGLES;JOLLIES;JULIETS;JURYRIG;KEELSON;KELLETS;KIBBERS;KICKING;KILLICK;KITCHEN;LADDERS;LANYARD;LATERAL;LAYDAYS;LAZARET;LEADING;LEEHELM;LEESIDE;LEEWARD;LIBERTY;LIGHTER;LIZARDS;LOADING;LOCKERS;LOFTING;LOLLING;LOOKOUT;LUBBERS;LUFFING;LUGGERS;LUGSAIL;MAEWEST;MAGENTA;MANAHOY;MANOWAR;MARCONI;MARINER;MARINES;MATELOT;MERCURY;MIZZENS;MONKEYS;MOORING;MOUSING;NARROWS;NATIONS;NETTING;NIPPERS;OFFCAST;OFFCLAW;OFFDECK;OFFICER;OFFPIER;OILSKIN;OLDSALT;ONBOARD;OREBOAT;OUTEAST;OUTHAUL;OUTLINE;OUTLYER;OUTWARD;OUTWEST;PAGEFIT;PAINTED;PAINTER;PANTING;PARCELS;PARLEYS;PARRELS;PASSAGE;PELAGIC;PENDANT;PENNANT;PEPPERS;PICKETS;PILOTED;PINNACE;PINTLES;PIRATES;PIVOTED;PLATING;POINTED;POINTER;POSITED;PRESENT;PREVENT;PRINTED;PURPLES;PURSERS;PURSUED;PUSSERS;QUARTER;QUAYING;QUEBECS;RABBETS;RADARED;RATLINE;RECKONS;REDUCED;REEFERS;REGALIA;REGENTS;REPAIRS;RETRACT;RIGGING;RIPRAPS;ROMPERS;ROOFING;ROWLOCK;RUDDERS;RUFFLES;RUMMAGE;RUNNING;SAGGING;SAILING;SAILORS;SALTIES;SALUTED;SALUTES;SALVORS;SAMPANS;SAMPSON;SCULLED;SCUPPER;SCUTTLE;SEACOCK;SEALING;SEASICK;SEEKERS;SERVING;SEXTANT;SHAKING;SHEATHS;SHELTER;SHIPPED;SHIPRIG;SHROUDS;SICKBAY;SIERRAS;SIGNALS;SILVERS;SKIPPER;SKYLARK;SKYSAIL;SLINGED;SLIPWAY;SNAGGED;SNOTTER;SOUNDER;SPACING;SPANKER;SPHERES;SPLICED;SPLICES;SPONSON;SPONSOR;SPRAYON;SPRINGS;SQUARES;STACKIE;STANDON;STARTER;STATION;STAYING;STEAMER;STEERED;STEEVES;STEPPED;STEWARD;STOPPER;STOVEIN;STOWAGE;STRIKER;STRIKES;STRIPED;STRIPES;SUMMERS;SUNFISH;SWIMMIE;SYSTEMS;TACKING;TACKLED;TACKLES;TAILEND;TANGOES;THEWIND;THWARTS;TILLERS;TIMBERS;TINCLAD;TOEROPE;TOGGLED;TOGGLES;TOMPION;TONNAGE;TOPMAST;TOPSAIL;TORPEDO;TOSSERS;TRADING;TRAFFIC;TRAMPER;TRANSIT;TRANSOM;TRAWLER;TRENAIL;TRENNEL;TRIMMER;TRIPPED;TRIPPER;TROOPER;TRUNNEL;TUGBOAT;TURNTWO;UNIFORM;UNSHIPS;UPBOUND;UPFRONT;UPNORTH;UPRISEN;UPSOUTH;URGENCY;UTCTIME;VARIATE;VBOTTOM;VESSELS;VICTORS;VICTORY;VOICING;VOYAGER;WATERED;WEATHER;WESTERN;WHALERS;WHARVES;WHELKIE;WHISKEY;WHISTLE;WINCHES;WINDAGE;WINTERS;WORKING;WRINKLE;YARDARM;ZOOMOUT"
        .Split(';');
    private static readonly string[] _compassDirections = @"N,NNE,NE,ENE,E,ESE,SE,SSE,S,SSW,SW,WSW,W,WNW,NW,NNW".Split(',');

    private static int _lastGeneratedRuleSeed;
    private static Sprite[] _lastGeneratedSprites;
    private static Flag[] _lastGeneratedFlags;
    private static Callsign[] _lastGeneratedCallsigns;

    protected override void DoStart(MonoRandom rnd)
    {
        // RULE SEED
        if (rnd.Seed != _lastGeneratedRuleSeed || _lastGeneratedFlags == null)
        {
            _lastGeneratedSprites = new Sprite[40];
            _lastGeneratedRuleSeed = rnd.Seed;
            _lastGeneratedFlags = GenerateFlags(rnd);

            if (rnd.Seed == 1)
                _lastGeneratedCallsigns = _seed1Callsigns;
            else
            {
                // Randomize callsigns
                var names = rnd.ShuffleFisherYates(_allCallsigns.ToArray()).Take(315).OrderBy(x => x).ToArray();
                var bearings = rnd.ShuffleFisherYates(Enumerable.Range(0, 360).ToArray());
                _lastGeneratedCallsigns = bearings.Take(315).Select((bearing, ix) => new Callsign { Name = names[ix], Bearing = bearing }).ToArray();
            }
        }
        // END OF RULE SEED



        FlagDisplay1.sprite = null;
        FlagDisplay2.sprite = null;

        _callsign = _lastGeneratedCallsigns[Rnd.Range(0, _lastGeneratedCallsigns.Length)];

        var finalBearing = Rnd.Range(0, 360);
        var flagsOnModule = new List<Sprite>();
        for (int i = 0; i < _callsign.Name.Length; i++)
        {
            var pos = i == 0 ? -1 : _callsign.Name.LastIndexOf(_callsign.Name[i], i - 1);
            if (pos != -1)  // repeater flag
                flagsOnModule.Add(GetFlagSprite(pos + 36));
            else if (_callsign.Name[i] >= '0' && _callsign.Name[i] <= '9')
                flagsOnModule.Add(GetFlagSprite(_callsign.Name[i] - '0' + 26));
            else
                flagsOnModule.Add(GetFlagSprite(_callsign.Name[i] - 'A'));
        }

        _bearingOnModule = (finalBearing - _callsign.Bearing + 360) % 360;
        var bearingOnModuleStr = _bearingOnModule.ToString();
        for (int i = 0; i < bearingOnModuleStr.Length; i++)
        {
            var pos = i == 0 ? -1 : bearingOnModuleStr.LastIndexOf(bearingOnModuleStr[i], i - 1);
            if (pos != -1)  // repeater flag
                flagsOnModule.Add(GetFlagSprite(pos + 36));
            else
                flagsOnModule.Add(GetFlagSprite(bearingOnModuleStr[i] - '0' + 26));
        }

        _flagsOnModule = flagsOnModule.ToArray();
        _curCompass = Rnd.Range(0, 16);

        _compassSolution = 0;
        var arr = new[] { 12, 34, 57, 79, 102, 124, 147, 169, 192, 214, 237, 259, 282, 304, 327, 349 };
        for (int i = 0; i < arr.Length; i++)
            if (finalBearing < arr[i])
            {
                _compassSolution = i;
                break;
            }

        StartCoroutine(ShowFlags());
        StartCoroutine(AlignCompass());
        Compass.OnInteract = CompassClicked;

        Log("Callsign in flags: {0}", _callsign.Name);
        Log("Bearing in flags: {0}", _bearingOnModule);
        Log("Bearing from callsign: {0}", _callsign.Bearing);
        Log("Final bearing: {0}", (_bearingOnModule + _callsign.Bearing) % 360);
        Log("Solution: {0}", _compassDirections[_compassSolution]);
    }

    private Sprite GetFlagSprite(int ix)
    {
        return _lastGeneratedSprites[ix] ?? (_lastGeneratedSprites[ix] = GenerateFlagSprite(_lastGeneratedFlags[ix]));
    }

    private IEnumerator AlignCompass()
    {
        while (!_isSolved)
        {
            CompassNeedle.localRotation = Quaternion.Lerp(CompassNeedle.localRotation, Quaternion.Euler(0, _curCompass * 360 / 16f, 0), 5 * Time.deltaTime);
            yield return null;
        }
    }

    private bool CompassClicked()
    {
        Compass.AddInteractionPunch();
        Audio.PlaySoundAtTransform("click", CompassNeedle);
        if (_isSolved)
            return false;
        _curCompass = (_curCompass + 1) % 16;
        if (_submit != null)
            StopCoroutine(_submit);
        _submit = StartCoroutine(Submit());
        return false;
    }

    private IEnumerator Submit()
    {
        yield return new WaitForSeconds(4.7f);
        if (_isSolved)
            yield break;
        Log("Submitted: {0}", _compassDirections[_curCompass]);
        if (_curCompass == _compassSolution)
        {
            Log("Module passed.");
            Module.HandlePass();
            _isSolved = true;
            Audio.PlaySoundAtTransform("solvesound", CompassNeedle);
        }
        else
        {
            Log("Strike!");
            Module.HandleStrike();
        }
    }

    private IEnumerator ShowFlags()
    {
        float duration = Rnd.Range(2.0f, 2.2f);
        float elapsed;

        yield return new WaitForSeconds(Rnd.Range(0, duration));
        _currentFlagIndex = Rnd.Range(0, _flagsOnModule.Length);

        FlagDisplay1.sprite = null;

        while (!_isSolved)
        {
            FlagDisplay2.sprite = _flagsOnModule[_currentFlagIndex];
            elapsed = 0f;
            while (elapsed < duration)
            {
                var t = (elapsed / duration);
                FlagDisplay1.transform.localPosition = new Vector3(0f + t * .02f, .01f, 0f + t * .1f);
                FlagDisplay2.transform.localPosition = new Vector3(-.02f + t * .02f, .01f, -.1f + t * .1f);
                yield return null;
                elapsed += Time.deltaTime;
            }

            _currentFlagIndex = (_currentFlagIndex + 1) % _flagsOnModule.Length;
            FlagDisplay1.sprite = FlagDisplay2.sprite;
        }

        FlagDisplay2.sprite = Solves[Rnd.Range(0, Solves.Length)];
        FlagDisplay2.transform.localScale = new Vector3(.01f, .01f, .01f);

        duration *= 2f;
        elapsed = 0f;
        while (elapsed < duration)
        {
            var t = (elapsed / duration);
            t *= (2 - t);
            FlagDisplay1.transform.localPosition = new Vector3(0f + t * .02f, .01f, 0f + t * .1f);
            FlagDisplay2.transform.localPosition = new Vector3(-.02f + t * .02f, .01f, -.1f + t * .1f);
            yield return null;
            elapsed += Time.deltaTime;
        }

        FlagDisplay1.gameObject.SetActive(false);
        FlagDisplay2.transform.localPosition = new Vector3(0, .01f, 0);
    }

#pragma warning disable 414
    private readonly string TwitchHelpMessage = @"!{0} N, !{0} NNE, etc.";
#pragma warning restore 414

    public IEnumerator ProcessTwitchCommand(string command)
    {
        command = command.Trim();
        for (int i = 0; i < _compassDirections.Length; i++)
            if (_compassDirections[i].Equals(command, StringComparison.InvariantCultureIgnoreCase))
            {
                yield return null;
                yield return Enumerable.Repeat(Compass, (i - _curCompass + 15) % 16 + 1).ToArray();
                yield return _curCompass == _compassSolution ? "solve" : "strike";
                yield break;
            }
    }

    public IEnumerator TwitchHandleForcedSolve()
    {
        do
        {
            Compass.OnInteract();
            yield return new WaitForSeconds(.1f);
        }
        while (_curCompass != _compassSolution);
        while (!_isSolved)
            yield return true;
    }
}
