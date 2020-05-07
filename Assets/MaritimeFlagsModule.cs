using System;
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
public class MaritimeFlagsModule : MonoBehaviour
{
    public KMBombInfo Bomb;
    public KMBombModule Module;
    public KMAudio Audio;

    public Sprite[] Solves;
    public SpriteRenderer FlagDisplay1;
    public SpriteRenderer FlagDisplay2;
    public KMSelectable Compass;
    public Transform CompassNeedle;
    public KMRuleSeedable RuleSeedable;

    private static int _moduleIdCounter = 1;
    private int _moduleId;
    private Callsign _callsign;
    private int _bearingOnModule;
    private int _compassSolution;
    private Sprite[] _flagsOnModule;
    private int _currentFlagIndex;
    private bool _isSolved;
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

    static T[] newArray<T>(params T[] array)
    {
        return array;
    }

    static T[] newArray<T>(int length, Func<int, T> fnc)
    {
        var arr = new T[length];
        for (int i = 0; i < length; i++)
            arr[i] = fnc(i);
        return arr;
    }

    private static readonly int[] _plussesDesign = new[] { 17, 27, 31, 32, 33, 41, 42, 43, 47, 57, 97, 111, 112, 113, 127, 167, 177, 181, 182, 183, 191, 192, 193, 197, 207 };
    private static readonly FlagDesign[] _flagDesigns = newArray(
        new FlagDesign { NameFmt = @"plain {0}", NumColors = 1, ReverseAllowed = false, CutoutAllowed = true, GetPixel = (x, y) => 0 },
        new FlagDesign { NameFmt = @"{0}-{1} vertical", NumColors = 2, ReverseAllowed = false, CutoutAllowed = true, GetPixel = (x, y) => x >= .5 ? 1 : 0 },
        new FlagDesign { NameFmt = @"{0}-{1}-{2} vertical", NumColors = 3, ReverseAllowed = false, CutoutAllowed = true, GetPixel = (x, y) => (int) (x * 3) % 3 },
        new FlagDesign { NameFmt = @"{1}-{0}-{1} vertical", NumColors = 2, ReverseAllowed = true, CutoutAllowed = true, GetPixel = (x, y) => ((int) (x * 3) % 2) ^ 1 },
        new FlagDesign { NameFmt = @"{0}-{1}-{0} vertical uneven", NumColors = 2, ReverseAllowed = true, CutoutAllowed = false, GetPixel = (x, y) => x >= .8 || x < .2 ? 0 : 1 },
        new FlagDesign { NameFmt = @"{0}-{1} 6 vertical stripes", NumColors = 2, ReverseAllowed = false, CutoutAllowed = false, GetPixel = (x, y) => (int) (x * 6) % 2 },
        new FlagDesign { NameFmt = @"{0}-{1} horizontal", NumColors = 2, ReverseAllowed = false, CutoutAllowed = true, GetPixel = (x, y) => y < .5 ? 1 : 0 },
        new FlagDesign { NameFmt = @"{0}-{1}-{2} horizontal", NumColors = 3, ReverseAllowed = false, CutoutAllowed = true, GetPixel = (x, y) => 2 - (int) (y * 3) % 3 },
        new FlagDesign { NameFmt = @"{1}-{0}-{1} horizontal", NumColors = 2, ReverseAllowed = true, CutoutAllowed = true, GetPixel = (x, y) => ((int) (y * 3) % 2) ^ 1 },
        new FlagDesign { NameFmt = @"{0}-{1}-{0} horizontal uneven", NumColors = 2, ReverseAllowed = true, CutoutAllowed = true, GetPixel = (x, y) => y >= .8 || y < .2 ? 0 : 1 },
        new FlagDesign { NameFmt = @"{0}-{1}-{2}-{1}-{0} horizontal", NumColors = 3, ReverseAllowed = true, CutoutAllowed = true, GetPixel = (x, y) => y >= .8 ? 0 : y >= .6 ? 1 : y >= .4 ? 2 : y >= .2 ? 1 : 0 },
        new FlagDesign { NameFmt = @"{0}-{1} 6 horizontal stripes", ReverseAllowed = false, NumColors = 2, CutoutAllowed = true, GetPixel = (x, y) => 1 - (int) (y * 6) % 2 },
        new FlagDesign { NameFmt = @"{1} diamond on {0}", ReverseAllowed = true, NumColors = 2, CutoutAllowed = false, GetPixel = (x, y) => Math.Abs(x - y) < .5 && Math.Abs(x + y - 1) < .5 ? 1 : 0 },
        new FlagDesign { NameFmt = @"{1} centered circle on {0}", ReverseAllowed = true, NumColors = 2, CutoutAllowed = false, GetPixel = (x, y) => Math.Pow(x - .5, 2) + Math.Pow(y - .5, 2) < .0625 ? 1 : 0 },
        new FlagDesign { NameFmt = @"{2} circle on {1} circle on {0}", ReverseAllowed = true, NumColors = 3, CutoutAllowed = false, GetPixel = (x, y) => { var d = Math.Pow(x - .5, 2) + Math.Pow(y - .5, 2); return d < .04 ? 2 : d < .16 ? 1 : 0; } },
        new FlagDesign { NameFmt = @"{1}-{0} 2×2 checkerboard", ReverseAllowed = false, NumColors = 2, CutoutAllowed = true, GetPixel = (x, y) => (x >= .5) ^ (y >= .5) ? 1 : 0 },
        new FlagDesign { NameFmt = @"{1}-{2} orthogonal quadrants on {0}", ReverseAllowed = false, NumColors = 3, CutoutAllowed = true, GetPixel = (x, y) => y < .5 ? 0 : x >= .5 ? 2 : 1 },
        new FlagDesign { NameFmt = @"{0}-{2}-{1}-{3} orthogonal quadrants", ReverseAllowed = false, NumColors = 4, CutoutAllowed = true, GetPixel = (x, y) => y < .5 ? (x >= .5 ? 1 : 3) : (x >= .5 ? 2 : 0) },
        new FlagDesign { NameFmt = @"{1}-{0} 4×4 checkerboard", ReverseAllowed = false, NumColors = 2, CutoutAllowed = false, GetPixel = (x, y) => ((int) (x * 4) % 2 == 0) ^ ((int) (y * 4) % 2 == 0) ? 1 : 0 },
        new FlagDesign { NameFmt = @"{1} saltire on {0}", ReverseAllowed = true, NumColors = 2, CutoutAllowed = false, GetPixel = (x, y) => Math.Abs(x - y) < .125 || Math.Abs(x + y - 1) < .125 ? 1 : 0 },
        new FlagDesign { NameFmt = @"{0}-{1} diagonal", ReverseAllowed = false, NumColors = 2, CutoutAllowed = false, GetPixel = (x, y) => y > 1 - x ? 1 : 0 },
        new FlagDesign { NameFmt = @"{1} centered square on {0}", ReverseAllowed = true, NumColors = 2, CutoutAllowed = true, GetPixel = (x, y) => (int) (x * 3) == 1 && (int) (y * 3) == 1 ? 1 : 0 },
        new FlagDesign { NameFmt = @"{2} square on {1} square on {0}", ReverseAllowed = true, NumColors = 3, CutoutAllowed = false, GetPixel = (x, y) => { var x1 = (int) (x * 5); var y1 = (int) (y * 5); return x1 == 2 && y1 == 2 ? 2 : x1 >= 1 && x1 <= 3 && y1 >= 1 && y1 <= 3 ? 1 : 0; } },
        new FlagDesign { NameFmt = @"{2} square on {0}-{1} diagonal", ReverseAllowed = false, NumColors = 3, CutoutAllowed = false, GetPixel = (x, y) => (int) (x * 3) == 1 && (int) (y * 3) == 1 ? 2 : (1 - y) > x ? 0 : 1 },
        new FlagDesign { NameFmt = @"{2} square on {0}-{1} horizontal", ReverseAllowed = false, NumColors = 3, CutoutAllowed = true, GetPixel = (x, y) => (int) (x * 3) == 1 && (int) (y * 3) == 1 ? 2 : y < .5 ? 1 : 0 },
        new FlagDesign { NameFmt = @"{1} cross on {0}", ReverseAllowed = true, NumColors = 2, CutoutAllowed = false, GetPixel = (x, y) => (int) (x * 5) == 2 || (int) (y * 5) == 2 ? 1 : 0 },
        new FlagDesign { NameFmt = @"{0}-{1} 7 diagonal stripes", ReverseAllowed = false, NumColors = 2, CutoutAllowed = false, GetPixel = (x, y) => (int) (3.5 * (x - y + 1)) % 2 == 0 ? 0 : 1 },
        new FlagDesign { NameFmt = @"{0}-{1} 10 diagonal stripes", ReverseAllowed = false, NumColors = 2, CutoutAllowed = false, GetPixel = (x, y) => (int) (7 * (x - y + 2)) % 2 == 0 ? 1 : 0 },
        new FlagDesign { NameFmt = @"{0}-{3}-{1}-{2} diagonal quadrants", ReverseAllowed = false, NumColors = 4, CutoutAllowed = false, GetPixel = (x, y) => y > x ? (y > 1 - x ? 0 : 2) : (y > 1 - x ? 3 : 1) },
        new FlagDesign { NameFmt = @"{0}-{1} horizontal semicircles pattern", ReverseAllowed = false, NumColors = 2, CutoutAllowed = true, GetPixel = (x, y) => (Math.Pow(x - .5, 2) + Math.Pow(y - .5, 2) < .0625) ^ (y >= .5) ? 0 : 1 },
        new FlagDesign { NameFmt = @"{0}-{1} diagonal semicircles pattern", ReverseAllowed = false, NumColors = 2, CutoutAllowed = false, GetPixel = (x, y) => (Math.Pow(x - .5, 2) + Math.Pow(y - .5, 2) < .0625) ^ (1 - y < x) ? 1 : 0 },
        new FlagDesign { NameFmt = @"{1} triangle on {0}", ReverseAllowed = true, NumColors = 2, CutoutAllowed = true, GetPixel = (x, y) => y > x * 3 / 7.5 + .2 && y < -x * 3 / 7.5 + .8 ? 1 : 0 },
        new FlagDesign { NameFmt = @"2 {1} triangles on {0}", ReverseAllowed = true, NumColors = 2, CutoutAllowed = false, GetPixel = (x, y) => (y > x / 4 && y < -x / 4 + .5) || (y > x / 4 + .5 && y < 1 - x / 4) ? 1 : 0 },
        new FlagDesign { NameFmt = @"{1} plusses on {0}", ReverseAllowed = true, NumColors = 2, CutoutAllowed = false, GetPixel = (x, y) => _plussesDesign.Contains((int) (x * 15) + 15 * ((int) (y * 15))) ? 1 : 0 },
        new FlagDesign { NameFmt = @"{1} square at edge on {0}", ReverseAllowed = true, NumColors = 2, CutoutAllowed = true, GetPixel = (x, y) => y >= .3 && y < .7 && x < .5 ? 1 : 0 },
        new FlagDesign { NameFmt = @"{0}-{1}-{2} triangle regions", ReverseAllowed = false, NumColors = 3, CutoutAllowed = false, GetPixel = (x, y) => x * 2 > y + 1 ? 2 : x / 2 > y - .5 ? 1 : 0 });
    private static readonly FlagDesign[] _repeaterDesigns = newArray(
        new FlagDesign { NameFmt = @"{1} triangle on {0}", IsRepeater = true, NumColors = 2, ReverseAllowed = true, GetPixel = (x, y) => 3.2 * (y - .1) > x && -3.2 * (y - .525) > x ? 1 : 0 },
        new FlagDesign { NameFmt = @"{0}-{1} vertical", IsRepeater = true, NumColors = 2, ReverseAllowed = true, GetPixel = (x, y) => x >= .5 ? 1 : 0 },
        new FlagDesign { NameFmt = @"{0}-{1}-{0} horizontal", IsRepeater = true, NumColors = 2, ReverseAllowed = true, GetPixel = (x, y) => y >= .2 && y < .425 ? 1 : 0 },
        new FlagDesign { NameFmt = @"{1} square on {0}", IsRepeater = true, NumColors = 2, ReverseAllowed = true, GetPixel = (x, y) => y >= .2 && y < .425 && x < .225 ? 1 : 0 });

    private static readonly ColorInfo[][] _colorGroups = newArray(
        new[] { new ColorInfo { Name = "red", Color = Color.red } },
        new[] { new ColorInfo { Name = "blue", Color = Color.blue }, new ColorInfo { Name = "black", Color = Color.black } },
        new[] { new ColorInfo { Name = "yellow", Color = new Color(1, 1, 0) }, new ColorInfo { Name = "white", Color = Color.white } });

    double distancePointToLine(double px, double py, double lx1, double ly1, double lx2, double ly2)
    {
        var dirX = lx2 - lx1;
        var dirY = ly2 - ly1;
        var lambda = (dirX * (px - lx1) + dirY * (py - ly1)) / (dirX * dirX + dirY * dirY);
        return Math.Sqrt(Math.Pow(px - (lx1 + lambda * dirX), 2) + Math.Pow(py - (ly1 + lambda * dirY), 2));
    }

    Sprite generateFlagSprite(int ix)
    {
        if (_lastGeneratedSprites[ix] != null)
            return _lastGeneratedSprites[ix];

        var flag = _lastGeneratedFlags[ix];

        const int w = 519;
        const int h = 519;
        const int padding = 16;
        const int thickness = 8;
        var tx = new Texture2D(w, h, TextureFormat.ARGB32, false);
        tx.SetPixels(newArray(w * h, i =>
        {
            var x = i % w;
            var y = i / w;

            if (flag.Design.IsRepeater)
            {
                const int pp = (h - (h - 2 * padding) * 5 / 8) / 2;
                // Left frame
                if (x > padding - thickness / 2 && x < padding + thickness / 2 && y > pp && y < h - pp)
                    return Color.black;
                // Top frame
                else if (x > padding - thickness / 2 && y <= w / 2 && distancePointToLine(x, y, padding, pp, w - padding, h / 2) < thickness / 2)
                    return Color.black;
                // Bottom frame
                else if (x > padding - thickness / 2 && y >= w / 2 && distancePointToLine(x, y, padding, h - pp, w - padding, h / 2) < thickness / 2)
                    return Color.black;
                // Flag body
                else if (x > padding && 3.2 * (y - pp) > x - padding && -3.2 * (y - h + pp) > x - padding)
                    return flag.Colors[flag.Design.GetPixel((x - padding) / (double) (w - 2 * padding), (y - pp) / (double) (h - 2 * padding))].Color;
            }
            else if (flag.Cutout)
            {
                const int iw = w - 2 * padding;
                // Top frame
                if (x > padding - thickness / 2 && x < w - padding && y > padding - thickness / 2 && y < padding + thickness / 2)
                    return Color.black;
                // Bottom frame
                else if (x > padding - thickness / 2 && x < w - padding && y > h - padding - thickness / 2 && y < h - padding + thickness / 2)
                    return Color.black;
                // Left frame
                else if (x > padding - thickness / 2 && x < padding + thickness / 2 && y > padding - thickness / 2 && y < h - padding + thickness / 2)
                    return Color.black;
                // Bottom half of right frame
                else if (y > padding - thickness / 2 && y <= h / 2 && Math.Abs(x - padding - iw * 3 / 4 - (-y + w / 2) / 2) < thickness / 2)
                    return Color.black;
                // Top half of right frame
                else if (y >= h / 2 && y < h - padding + thickness / 2 && Math.Abs(x - padding - iw * 3 / 4 - (y - w / 2) / 2) < thickness / 2)
                    return Color.black;
                // Flag body
                else if (x > padding && y > padding && y < h - padding && ((x - padding - iw * 3 / 4) < (y - w / 2) / 2 || (x - padding - iw * 3 / 4) < (-y + w / 2) / 2))
                    return flag.Colors[flag.Design.GetPixel((x - padding) / (double) (w - 2 * padding), (y - padding) / (double) (h - 2 * padding))].Color;
            }
            else
            {
                // Border
                if (x > padding - thickness / 2 && x < w - padding + thickness / 2 && y > padding - thickness / 2 && y < h - padding + thickness / 2 &&
                    !(x > padding + thickness / 2 && x < w - padding - thickness / 2 && y > padding + thickness / 2 && y < h - padding - thickness / 2))
                    return Color.black;
                // Flag body
                else if (x > padding && x < w - padding && y > padding && y < h - padding)
                    return flag.Colors[flag.Design.GetPixel((x - padding) / (double) (w - 2 * padding), (y - padding) / (double) (h - 2 * padding))].Color;
            }
            return new Color(0, 0, 0, 0);
        }));
        tx.Apply();
        Debug.LogFormat(@"<Maritime Flags #{0}> Generated flag sprite for: {1}", _moduleId, ix < 26 ? ((char) ('A' + ix)).ToString() : ix < 36 ? ((char) ('0' + ix - 26)).ToString() : "R" + (ix - 36 + 1));
        _lastGeneratedSprites[ix] = Sprite.Create(tx, new Rect(0, 0, w, h), new Vector2(.5f, .5f));
        return _lastGeneratedSprites[ix];
    }

    void Start()
    {
        _moduleId = _moduleIdCounter++;
        FlagDisplay1.sprite = null;
        FlagDisplay2.sprite = null;

        var red = _colorGroups[0][0];
        var blue = _colorGroups[1][0];
        var black = _colorGroups[1][1];
        var yellow = _colorGroups[2][0];
        var white = _colorGroups[2][1];

        var rnd = RuleSeedable.GetRNG();
        Debug.LogFormat("[Maritime Flags #{0}] Using rule seed: {1}", _moduleId, rnd.Seed);

        if (rnd.Seed != _lastGeneratedRuleSeed || _lastGeneratedFlags == null)
        {
            _lastGeneratedSprites = new Sprite[40];
            _lastGeneratedRuleSeed = rnd.Seed;

            if (rnd.Seed == 1)
            {
                _lastGeneratedFlags = new Flag[]
                {
                    new Flag(_flagDesigns[1], new[] { white, blue }, cutout: true),
                    new Flag(_flagDesigns[0], new[] { red }, cutout: true),
                    new Flag(_flagDesigns[10], new[] { blue, white, red }),
                    new Flag(_flagDesigns[9], new[] { yellow, blue }),
                    new Flag(_flagDesigns[6], new[] { blue, red }),
                    new Flag(_flagDesigns[12], new[] { white, red }),
                    new Flag(_flagDesigns[5], new[] { yellow, blue }),
                    new Flag(_flagDesigns[1], new[] { white, red }),
                    new Flag(_flagDesigns[13], new[] { yellow, black }),
                    new Flag(_flagDesigns[8], new[] { white, blue }),
                    new Flag(_flagDesigns[1], new[] { yellow, blue }),
                    new Flag(_flagDesigns[15], new[] { black, yellow }),
                    new Flag(_flagDesigns[19], new[] { blue, white }),
                    new Flag(_flagDesigns[18], new[] { white, blue }),
                    new Flag(_flagDesigns[20], new[] { yellow, red }),
                    new Flag(_flagDesigns[21], new[] { blue, white }),
                    new Flag(_flagDesigns[0], new[] { yellow }),
                    new Flag(_flagDesigns[25], new[] { red, yellow }),
                    new Flag(_flagDesigns[21], new[] { white, blue }),
                    new Flag(_flagDesigns[2], new[] { red, white, blue }),
                    new Flag(_flagDesigns[15], new[] { white, red }),
                    new Flag(_flagDesigns[19], new[] { white, red }),
                    new Flag(_flagDesigns[22], new[] { blue, white, red }),
                    new Flag(_flagDesigns[25], new[] { white, blue }),
                    new Flag(_flagDesigns[27], new[] { yellow, red }),
                    new Flag(_flagDesigns[28], new[] { yellow, red, black, blue }),
                    new Flag(_flagDesigns[33], new[] { white, blue }),
                    new Flag(_flagDesigns[8], new[] { yellow, red }),
                    new Flag(_flagDesigns[8], new[] { red, yellow }),
                    new Flag(_flagDesigns[8], new[] { red, blue }),
                    new Flag(_flagDesigns[19], new[] { red, white }),
                    new Flag(_flagDesigns[19], new[] { yellow, blue }),
                    new Flag(_flagDesigns[26], new[] { white, blue }),
                    new Flag(_flagDesigns[3], new[] { white, red }),
                    new Flag(_flagDesigns[3], new[] { blue, yellow }),
                    new Flag(_flagDesigns[3], new[] { white, blue }),
                    new Flag(_repeaterDesigns[0], new[] { blue, yellow }),
                    new Flag(_repeaterDesigns[1], new[] { blue, white }),
                    new Flag(_repeaterDesigns[2], new[] { white, black }),
                    new Flag(_repeaterDesigns[3], new[] { red, yellow })
                };
                _lastGeneratedCallsigns = _seed1Callsigns;
            }
            else
            {
                _lastGeneratedFlags = new Flag[40];
                if (rnd.Seed == 0)
                {
                    for (var k = 0; k < 2; k++)
                    {
                        var designs = k == 0 ? _flagDesigns : _repeaterDesigns;
                        for (var i = 0; i < designs.Length; i++)
                        {
                            var colors = new ColorInfo[designs[i].NumColors];
                            for (var j = 0; j < designs[i].NumColors; j++)
                            {
                                var n = (float) (designs[i].NumColors == 1 ? .5 : .8 * j / (designs[i].NumColors - 1) + .1);
                                colors[j] = new ColorInfo { Color = new Color(n, n, n), Name = "Gray " + n };
                            }
                            _lastGeneratedFlags[(k == 1) ? 36 + i : i] = new Flag(designs[i], colors, designs[i].CutoutAllowed);
                        }
                    }
                }
                else
                {
                    var letterNumberFlags = generateFlags(36, _flagDesigns, rnd, "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".Select(ch => ch.ToString()).ToArray());
                    var repeaterFlags = generateFlags(4, _repeaterDesigns, rnd, Enumerable.Range(1, 4).Select(i => "Repeat " + i).ToArray());
                    _lastGeneratedFlags = letterNumberFlags.Concat(repeaterFlags).ToArray();
                }

                // Randomize callsigns
                var names = rnd.ShuffleFisherYates(_allCallsigns.ToArray()).Take(315).OrderBy(x => x).ToArray();
                var bearings = rnd.ShuffleFisherYates(Enumerable.Range(0, 360).ToArray());
                _lastGeneratedCallsigns = bearings.Take(315).Select((bearing, ix) => new Callsign { Name = names[ix], Bearing = bearing }).ToArray();
            }
        }

        _callsign = _lastGeneratedCallsigns[Rnd.Range(0, _lastGeneratedCallsigns.Length)];

        var finalBearing = Rnd.Range(0, 360);
        var flagsOnModule = new List<Sprite>();
        for (int i = 0; i < _callsign.Name.Length; i++)
        {
            var pos = i == 0 ? -1 : _callsign.Name.LastIndexOf(_callsign.Name[i], i - 1);
            if (pos != -1)  // repeater flag
                flagsOnModule.Add(generateFlagSprite(pos + 36));
            else if (_callsign.Name[i] >= '0' && _callsign.Name[i] <= '9')
                flagsOnModule.Add(generateFlagSprite(_callsign.Name[i] - '0' + 26));
            else
                flagsOnModule.Add(generateFlagSprite(_callsign.Name[i] - 'A'));
        }

        _bearingOnModule = (finalBearing - _callsign.Bearing + 360) % 360;
        var bearingOnModuleStr = _bearingOnModule.ToString();
        for (int i = 0; i < bearingOnModuleStr.Length; i++)
        {
            var pos = i == 0 ? -1 : bearingOnModuleStr.LastIndexOf(bearingOnModuleStr[i], i - 1);
            if (pos != -1)  // repeater flag
                flagsOnModule.Add(generateFlagSprite(pos + 36));
            else
                flagsOnModule.Add(generateFlagSprite(bearingOnModuleStr[i] - '0' + 26));
        }

        _flagsOnModule = flagsOnModule.ToArray();
        _curCompass = Rnd.Range(0, 16);
        _isSolved = false;

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

        Debug.LogFormat(@"[Maritime Flags #{0}] Callsign in flags: {1}", _moduleId, _callsign.Name);
        Debug.LogFormat(@"[Maritime Flags #{0}] Bearing in flags: {1}", _moduleId, _bearingOnModule);
        Debug.LogFormat(@"[Maritime Flags #{0}] Bearing from callsign: {1}", _moduleId, _callsign.Bearing);
        Debug.LogFormat(@"[Maritime Flags #{0}] Final bearing: {1}", _moduleId, (_bearingOnModule + _callsign.Bearing) % 360);
        Debug.LogFormat(@"[Maritime Flags #{0}] Solution: {1}", _moduleId, _compassDirections[_compassSolution]);
    }

    private List<Flag> generateFlags(int count, FlagDesign[] designs, MonoRandom rnd, string[] flagNames)
    {
        // Each design can be used different numbers of times
        var designIxsAvailable = new List<int>();
        for (var i = 0; i < designs.Length; i++)
        {
            var allowed =
                designs.Length == 4 ? 1 :
                designs[i].NumColors == 4 ? 1 :
                designs[i].NumColors == 2 && designs[i].ReverseAllowed ? 4 : 3;
            for (var j = 0; j < allowed; j++)
                designIxsAvailable.Add(i);
        }

        // Assign designs at random
        var flags = new List<Flag>();
        var colorCombinations = new Dictionary<int, List<int[]>>();
        var availableColorIxs = Enumerable.Range(0, _colorGroups.Length).ToList();
        for (var i = 0; i < count; i++)
        {
            var ix = rnd.Next(0, designIxsAvailable.Count);
            var designIx = designIxsAvailable[ix];
            designIxsAvailable.RemoveAt(ix);

            ColorInfo[] flagColors;

            if (designs[designIx].NumColors == 4)
            {
                // 4-color designs are allowed to have blue+black and/or yellow+white, but only in a specific order.
                flagColors = new ColorInfo[4];
                var fcIx = 0;
                rnd.ShuffleFisherYates(availableColorIxs);
                var ixIx = 0;
                while (fcIx < 4)
                {
                    for (var j = 0; j < _colorGroups[availableColorIxs[ixIx]].Length && fcIx < 4; j++)
                    {
                        flagColors[fcIx] = _colorGroups[availableColorIxs[ixIx]][j];
                        fcIx++;
                        if (fcIx % 2 == 0)
                            break;
                    }
                    ixIx++;
                }
            }
            else
            {
                // Find a random color combination that hasn’t been used yet.
                // Use only one color per group so we don’t get black+blue or yellow+white on the same flag.
                int[] colorIxs;
                do
                {
                    rnd.ShuffleFisherYates(availableColorIxs);
                    colorIxs = availableColorIxs.Take(designs[designIx].NumColors).ToArray();
                }
                while (colorCombinations.ContainsKey(designIx) && colorCombinations[designIx].Any(cc => cc.SequenceEqual(colorIxs) || (!designs[designIx].ReverseAllowed && cc.Reverse().SequenceEqual(colorIxs))));
                if (!colorCombinations.ContainsKey(designIx))
                    colorCombinations[designIx] = new List<int[]>();
                colorCombinations[designIx].Add(colorIxs);

                do
                    flagColors = colorIxs.Select(cix => _colorGroups[cix][rnd.Next(0, _colorGroups[cix].Length)]).ToArray();
                // Special case: don’t allow entirely black-and-white flags
                while (designs[designIx].NumColors == 2 && ((flagColors[0].Name == "black" && flagColors[1].Name == "white") || (flagColors[0].Name == "white" && flagColors[1].Name == "black")));
            }
            var cutout = designs[designIx].CutoutAllowed && (rnd.Next(0, 10) == 0);
            Debug.LogFormat(@"<Maritime Flags #{0}> Flag {1} is {2}{3}", _moduleId, flagNames[i], string.Format(designs[designIx].NameFmt, flagColors.Select(cc => (object) cc.Name).ToArray()), cutout ? " with cutout" : "");
            flags.Add(new Flag(designs[designIx], flagColors, cutout: cutout));
        }
        return flags;
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
        Debug.LogFormat(@"[Maritime Flags #{0}] Submitted: {1}", _moduleId, _compassDirections[_curCompass]);
        if (_curCompass == _compassSolution)
        {
            Debug.LogFormat(@"[Maritime Flags #{0}] Module passed.", _moduleId);
            Module.HandlePass();
            _isSolved = true;
            Audio.PlaySoundAtTransform("solvesound", CompassNeedle);
        }
        else
        {
            Debug.LogFormat(@"[Maritime Flags #{0}] Strike!", _moduleId);
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
            t = t * (2 - t);
            FlagDisplay1.transform.localPosition = new Vector3(0f + t * .02f, .01f, 0f + t * .1f);
            FlagDisplay2.transform.localPosition = new Vector3(-.02f + t * .02f, .01f, -.1f + t * .1f);
            yield return null;
            elapsed += Time.deltaTime;
        }

        FlagDisplay1.gameObject.SetActive(false);
        FlagDisplay2.transform.localPosition = new Vector3(0, .01f, 0);
    }

#pragma warning disable 414
    private readonly string TwitchHelpMessage = @"Set the compass with “!{0} N”, “!{0} NNE”, etc.";
#pragma warning restore 414

    public IEnumerator ProcessTwitchCommand(string command)
    {
        for (int i = 0; i < _compassDirections.Length; i++)
            if (_compassDirections[i].Equals(command, StringComparison.InvariantCultureIgnoreCase))
            {
                yield return null;
                yield return Enumerable.Repeat(Compass, (i - _curCompass + 15) % 16 + 1).ToArray();
                yield return _curCompass == _compassSolution ? "solve" : "strike";
                yield break;
            }
    }

    IEnumerator TwitchHandleForcedSolve()
    {
        while (_curCompass != _compassSolution)
        {
            Compass.OnInteract();
            yield return new WaitForSeconds(.1f);
        }
        while (!_isSolved)
            yield return true;
    }
}
