using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using MaritimeFlags;
using UnityEngine;
using Rnd = UnityEngine.Random;

/// <summary>
/// On the Subject of Maritime Semaphore
/// Created by Timwi
/// </summary>
public class MaritimeSemaphoreModule : MaritimeBase
{
    protected override string Name { get { return "Maritime Semaphore"; } }

    public MeshRenderer LeftFlag;
    public MeshRenderer LeftFlag2;
    public MeshRenderer RightFlag;
    public MeshRenderer RightFlag2;
    public Material FlagMaterial;

    public Transform LeftBase;
    public Transform LeftPole;
    public Transform RightBase;
    public Transform RightPole;
    public Transform LeftFlagObj;
    public Transform RightFlagObj;

    public KMSelectable LeftButton;
    public KMSelectable RightButton;

    private static readonly int[] _semaphoreLeftFlagOrientations = new[] { 135, 90, 45, 0, 180, 180, 180, 90, 135, 0, 135, 135, 135, 135, 90, 90, 90, 90, 90, 45, 45, 0, -45, -45, 45, 225 };
    private static readonly int[] _semaphoreRightFlagOrientations = new[] { -180, -180, -180, -180, -45, -90, -135, -225, 45, -90, 0, -45, -90, -135, 45, 0, -45, -90, -135, 0, -45, -135, -90, -135, -90, -90 };

    private Material _leftMaterial;
    private Material _rightMaterial;
    private FlagInfo[] _flagInfos;
    private int _curPos;
    private int _solution;
    private Coroutine _transition;
    private Coroutine _submit;

    private static int _lastGeneratedRuleSeed;
    private static Texture2D[] _lastGeneratedTextures;
    private static Flag[] _lastGeneratedFlags;
    private static int[] _table;

    private const int _numSlots = 6;

    struct FlagInfo : IEquatable<FlagInfo>
    {
        public int LeftMaritime;
        public int RightMaritime;
        public int Semaphore;
        public bool IsInvalid
        {
            get
            {
                return LeftMaritime == Semaphore || RightMaritime == Semaphore ||
                    Math.Abs(LeftMaritime - Semaphore) > 10 || Math.Abs(RightMaritime - Semaphore) > 10 || Math.Sign(LeftMaritime - Semaphore) == Math.Sign(RightMaritime - Semaphore);
            }
        }
        public bool IsDummy
        {
            get
            {
                return LeftMaritime >= 26 || RightMaritime >= 26;
            }
        }

        public static FlagInfo Generate(bool avoidDummy)
        {
            while (true)
            {
                var fi = new FlagInfo { LeftMaritime = Rnd.Range(0, 36), RightMaritime = Rnd.Range(0, 36), Semaphore = Rnd.Range(0, 26) };
                if (!fi.IsInvalid && (!avoidDummy || !fi.IsDummy))
                    return fi;
            }
        }

        public bool Equals(FlagInfo other) { return other.LeftMaritime == LeftMaritime && other.RightMaritime == RightMaritime && other.Semaphore == Semaphore; }
        public override bool Equals(object obj) { return obj is FlagInfo && Equals((FlagInfo) obj); }
        public override int GetHashCode() { return LeftMaritime + 47 * RightMaritime + 2347 * Semaphore; }
    }

    protected override void DoStart(MonoRandom rnd)
    {
        // RULE SEED
        if (rnd.Seed != _lastGeneratedRuleSeed || _lastGeneratedFlags == null)
        {
            _lastGeneratedTextures = new Texture2D[40];
            _lastGeneratedRuleSeed = rnd.Seed;
            _lastGeneratedFlags = GenerateFlags(rnd);

            _table = new int[2 * 10 * _numSlots];
            for (var i = 0; i < 2 * _numSlots; i++)
            {
                var nums = Enumerable.Range(0, 10).ToArray();
                rnd.ShuffleFisherYates(nums);
                Array.Copy(nums, 0, _table, 10 * i, 10);
            }
            DebugLog("Generated table for rule seed {0}: {1}", rnd.Seed, _table.Join(", "));
        }
        // END OF RULE SEED


        _leftMaterial = LeftFlag.sharedMaterial = LeftFlag2.sharedMaterial = new Material(FlagMaterial);
        _rightMaterial = RightFlag.sharedMaterial = RightFlag2.sharedMaterial = new Material(FlagMaterial);

        var iter = 0;
        var flagInfos = new HashSet<FlagInfo>();
        tryAgain:
        iter++;
        flagInfos.Clear();
        var hasDummy = false;
        while (flagInfos.Count < _numSlots)
        {
            var fi = FlagInfo.Generate(hasDummy);
            if (fi.IsDummy)
                hasDummy = true;
            flagInfos.Add(fi);
        }
        if (!hasDummy)
            goto tryAgain;

        _flagInfos = flagInfos.ToArray().Shuffle();

        var values = new List<int>();
        for (var i = 0; i < _numSlots; i++)
        {
            var fi = _flagInfos[i];
            var val = 0;
            if (fi.IsDummy)
            {
                if (fi.LeftMaritime >= 26)
                    val += fi.LeftMaritime - 26;
                if (fi.RightMaritime >= 26)
                    val += fi.RightMaritime - 26;
            }
            else if (fi.LeftMaritime < fi.Semaphore)
            {
                val += _table[20 * i + 10 - (fi.Semaphore - fi.LeftMaritime)];
                val += _table[20 * i + 9 + (fi.RightMaritime - fi.Semaphore)];
            }
            else
            {
                val += _table[20 * i + 10 - (fi.Semaphore - fi.RightMaritime)];
                val += _table[20 * i + 9 + (fi.LeftMaritime - fi.Semaphore)];
            }
            values.Add(val);
        }

        _solution = (values.Sum() + _numSlots - 1) % _numSlots;   // −1 to compensate for the player’s 1-based counting
        if (_flagInfos[_solution].IsDummy)
            goto tryAgain;

        var alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        for (var i = 0; i < _numSlots; i++)
            Log("Position {0}: Left Maritime = {1}, Right Maritime = {2}, Semaphore = {3} (+{4}){5}", i + 1, alphabet[_flagInfos[i].LeftMaritime], alphabet[_flagInfos[i].RightMaritime], alphabet[_flagInfos[i].Semaphore],
                values[i], i == _solution ? " (SOLUTION)" : _flagInfos[i].IsDummy ? " (DUMMY)" : "");

        setPos(0, true);
        LeftButton.OnInteract += ButtonPress(LeftButton, -1);
        RightButton.OnInteract += ButtonPress(RightButton, 1);
        StartCoroutine(CheckTP());
    }

    private IEnumerator CheckTP()
    {
        yield return null;
        if (TwitchPlaysActive)
            Log("Twitch Plays mode is active.");
    }

    private KMSelectable.OnInteractHandler ButtonPress(KMSelectable btn, int offset)
    {
        return delegate
        {
            btn.AddInteractionPunch(.5f);
            Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, btn.transform);
            if (_isSolved)
                return false;
            setPos(_curPos + offset);
            if (_submit != null)
                StopCoroutine(_submit);
            if (!TwitchPlaysActive)
                _submit = StartCoroutine(submit());
            return false;
        };
    }

    private IEnumerator submit()
    {
        yield return new WaitForSeconds(10f);
        _submit = null;

        if (_curPos == _solution)
        {
            Log("Module solved!");
            Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.CorrectChime, transform);
            Module.HandlePass();
            _isSolved = true;
        }
        else if (!_flagInfos[_curPos].IsDummy && !_isSolved)
        {
            Log("You stayed on position {0} for 10 seconds. Strike!", _curPos + 1);
            Module.HandleStrike();
        }
    }

    private void setPos(int pos, bool forced = false)
    {
        var newPos = Math.Max(0, Math.Min(_numSlots - 1, pos));
        if (newPos == _curPos && !forced)
            return;
        _curPos = newPos;
        if (_transition != null)
            StopCoroutine(_transition);

        var leftSema = _semaphoreLeftFlagOrientations[_flagInfos[_curPos].Semaphore];
        var rightSema = _semaphoreRightFlagOrientations[_flagInfos[_curPos].Semaphore];
        _transition = StartCoroutine(transition(
            GetFlagTexture(_flagInfos[_curPos].LeftMaritime),
            GetFlagTexture(_flagInfos[_curPos].RightMaritime),
            -leftSema,
            -rightSema,
            leftSema < 0 || leftSema > 180 ? 0 : 180,
            rightSema > 0 || rightSema < -180 ? 180 : 0));
    }

    private Texture2D GetFlagTexture(int ix)
    {
        return _lastGeneratedTextures[ix] ?? (_lastGeneratedTextures[ix] = GenerateFlagTexture(_lastGeneratedFlags[ix]));
    }

    float _lastLeftFlagRot, _lastRightFlagRot, _lastLeftPoleRot, _lastRightPoleRot, _lastLeftScale, _lastRightScale;

    private IEnumerator transition(Texture2D leftTexture, Texture2D rightTexture, int leftFlagRot, int rightFlagRot, int leftPoleRot, int rightPoleRot)
    {
        var leftFlagStart = _lastLeftFlagRot;
        var rightFlagStart = _lastRightFlagRot;
        var leftPoleStart = _lastLeftPoleRot;
        var rightPoleStart = _lastRightPoleRot;
        var leftStartTexture = _leftMaterial.mainTexture;
        var rightStartTexture = _rightMaterial.mainTexture;
        var leftStartScale = _lastLeftScale;
        var rightStartScale = _lastRightScale;

        var elapsed = 0f;
        var duration = .2f;
        while (elapsed < duration)
        {
            LeftBase.localEulerAngles = new Vector3(0, _lastLeftFlagRot = Mathf.Lerp(leftFlagStart, leftFlagRot, elapsed / duration), 0);
            RightBase.localEulerAngles = new Vector3(0, _lastRightFlagRot = Mathf.Lerp(rightFlagStart, rightFlagRot, elapsed / duration), 0);
            LeftPole.localEulerAngles = new Vector3(0, 0, _lastLeftPoleRot = Mathf.Lerp(leftPoleStart, leftPoleRot, elapsed / duration));
            RightPole.localEulerAngles = new Vector3(0, 0, _lastRightPoleRot = Mathf.Lerp(rightPoleStart, rightPoleRot, elapsed / duration));

            var firstHalf = elapsed < duration * .5f;
            LeftFlagObj.localScale = firstHalf ? new Vector3(_lastLeftScale = Mathf.Lerp(leftStartScale, 0, 2 * elapsed / duration), 1, 1) : new Vector3(_lastLeftScale = Mathf.Lerp(0, 1, 2 * elapsed / duration - 1), 1, 1);
            RightFlagObj.localScale = firstHalf ? new Vector3(_lastRightScale = Mathf.Lerp(rightStartScale, 0, 2 * elapsed / duration), 1, 1) : new Vector3(_lastRightScale = Mathf.Lerp(0, 1, 2 * elapsed / duration - 1), 1, 1);
            _leftMaterial.mainTexture = firstHalf ? leftStartTexture : leftTexture;
            _rightMaterial.mainTexture = firstHalf ? rightStartTexture : rightTexture;
            yield return null;
            elapsed += Time.deltaTime;
        }

        LeftBase.localEulerAngles = new Vector3(0, _lastLeftFlagRot = leftFlagRot, 0);
        RightBase.localEulerAngles = new Vector3(0, _lastRightFlagRot = rightFlagRot, 0);
        LeftPole.localEulerAngles = new Vector3(0, 0, _lastLeftPoleRot = leftPoleRot);
        RightPole.localEulerAngles = new Vector3(0, 0, _lastRightPoleRot = rightPoleRot);
        LeftFlagObj.localScale = new Vector3(_lastLeftScale = 1, 1, 1);
        RightFlagObj.localScale = new Vector3(_lastRightScale = 1, 1, 1);
        _leftMaterial.mainTexture = leftTexture;
        _rightMaterial.mainTexture = rightTexture;
        _transition = null;
    }

#pragma warning disable 414
    private bool TwitchPlaysActive = false;
    private readonly string TwitchHelpMessage = @"!{0} next | !{0} prev | !{0} dummy [identify the dummy] | !{0} set 3 [move to position 3] | !{0} submit";
    private bool _twitchDummyIdentified = false;
#pragma warning restore 414

    public IEnumerator ProcessTwitchCommand(string command)
    {
        Match m;
        int i;

        if ((m = Regex.Match(command, @"^\s*(?:(?<n>next|n|right|r)|prev|p|left|l)\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)).Success)
        {
            yield return null;
            if (_flagInfos[_curPos].IsDummy && !_twitchDummyIdentified)
            {
                Log("TP: Strike because you tried to move off of the dummy without typing “!# dummy” first.");
                Module.HandleStrike();
                yield break;
            }
            yield return new[] { m.Groups["n"].Success ? RightButton : LeftButton };
            yield break;
        }
        else if (Regex.IsMatch(command, @"^\s*(?:dummy|d|identify|mark)\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant))
        {
            yield return null;
            if (!_flagInfos[_curPos].IsDummy)
            {
                Log("TP: Strike because you typed “!# dummy” when you weren’t on the dummy.");
                Module.HandleStrike();
                yield break;
            }
            _twitchDummyIdentified = true;
            yield break;
        }
        else if ((m = Regex.Match(command, @"^\s*(?:set|s)\s+(\d+)\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)).Success && int.TryParse(m.Groups[1].Value, out i) && i >= 1 && i <= _numSlots)
        {
            yield return null;
            i--;
            if (_flagInfos[_curPos].IsDummy && i != _curPos && !_twitchDummyIdentified)
            {
                Log("TP: Strike because you tried to move off of the dummy without typing “!# dummy” first.");
                Module.HandleStrike();
                yield break;
            }
            while (_curPos < i)
            {
                setPos(_curPos + 1);
                yield return new WaitForSeconds(.25f);
            }
            while (_curPos > i)
            {
                setPos(_curPos - 1);
                yield return new WaitForSeconds(.25f);
            }
            yield break;
        }
        else if (Regex.IsMatch(command, @"^\s*submit\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant))
        {
            yield return null;
            if (_submit != null)
                StopCoroutine(_submit);
            if (!_flagInfos[_curPos].IsDummy)
            {
                _submit = StartCoroutine(submit());
                yield return _curPos == _solution ? "solve" : "strike";
            }
            yield break;
        }
    }

    IEnumerator TwitchHandleForcedSolve()
    {
        while (_curPos < _solution)
        {
            setPos(_curPos + 1);
            yield return new WaitForSeconds(.25f);
        }
        while (_curPos > _solution)
        {
            setPos(_curPos - 1);
            yield return new WaitForSeconds(.25f);
        }
        if (_submit != null)
            StopCoroutine(_submit);
        _submit = StartCoroutine(submit());
        while (!_isSolved)
            yield return true;
    }
}
