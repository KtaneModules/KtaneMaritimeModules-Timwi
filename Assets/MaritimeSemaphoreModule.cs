using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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
    private static int[][] _table;
    private FlagInfo[] _flagInfos;
    private int _curPos;
    private int _solution;
    private Coroutine _transition;
    private Coroutine _submit;

    protected override void DoRuleseed(MonoRandom rnd)
    {
        _table = new int[8][];
        for (var i = 0; i < 8; i++)
        {
            _table[i] = new int[20];
            for (var j = 0; j < 20; j++)
                _table[i][j] = rnd.Next(1, 10);
        }
    }

    struct FlagInfo : IEquatable<FlagInfo>
    {
        public int LeftMaritime;
        public int RightMaritime;
        public int Semaphore;
        public bool IsDummy
        {
            get
            {
                return LeftMaritime == Semaphore || RightMaritime == Semaphore || LeftMaritime >= 26 || RightMaritime >= 26 ||
                    Math.Abs(LeftMaritime - Semaphore) > 10 || Math.Abs(RightMaritime - Semaphore) > 10 || Math.Sign(LeftMaritime - Semaphore) == Math.Sign(RightMaritime - Semaphore);
            }
        }

        public static FlagInfo Generate(bool avoidDummy)
        {
            while (true)
            {
                var fi = new FlagInfo { LeftMaritime = Rnd.Range(0, 36), RightMaritime = Rnd.Range(0, 36), Semaphore = Rnd.Range(0, 26) };
                if (!avoidDummy || !fi.IsDummy)
                    return fi;
            }
        }

        public bool Equals(FlagInfo other) { return other.LeftMaritime == LeftMaritime && other.RightMaritime == RightMaritime && other.Semaphore == Semaphore; }
        public override bool Equals(object obj) { return obj is FlagInfo && Equals((FlagInfo) obj); }
        public override int GetHashCode() { return LeftMaritime + 47 * RightMaritime + 2347 * Semaphore; }
    }

    protected override void DoStart()
    {
        _leftMaterial = LeftFlag.sharedMaterial = LeftFlag2.sharedMaterial = new Material(FlagMaterial);
        _rightMaterial = RightFlag.sharedMaterial = RightFlag2.sharedMaterial = new Material(FlagMaterial);

        var iter = 0;
        var flagInfos = new HashSet<FlagInfo>();
        tryAgain:
        iter++;
        flagInfos.Clear();
        var hasDummy = false;
        while (flagInfos.Count < 8)
        {
            var fi = FlagInfo.Generate(hasDummy);
            if (fi.IsDummy)
                hasDummy = true;
            flagInfos.Add(fi);
        }
        if (!hasDummy)
            goto tryAgain;

        _flagInfos = flagInfos.ToArray().Shuffle();

        var solutionLeft = 0;
        var solutionRight = 0;
        for (var i = 0; i < _flagInfos.Length; i++)
        {
            var fi = _flagInfos[i];
            if (fi.IsDummy)
                continue;
            if (fi.LeftMaritime < fi.Semaphore)
            {
                solutionLeft += _table[i][10 - (fi.Semaphore - fi.LeftMaritime)];
                solutionRight += _table[i][9 + (fi.RightMaritime - fi.Semaphore)];
            }
            else
            {
                solutionLeft += _table[i][10 - (fi.Semaphore - fi.RightMaritime)];
                solutionRight += _table[i][9 + (fi.LeftMaritime - fi.Semaphore)];
            }
        }

        _solution = ((solutionLeft - solutionRight) % 8 + 8) % 8;
        if (_flagInfos[_solution].IsDummy)
            goto tryAgain;

        var alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        for (var i = 0; i < _flagInfos.Length; i++)
            Log("Position {0}: Left Maritime = {1}, Right Maritime = {2}, Semaphore = {3}{4}", i + 1, alphabet[_flagInfos[i].LeftMaritime], alphabet[_flagInfos[i].RightMaritime], alphabet[_flagInfos[i].Semaphore], i == _solution ? " (SOLUTION)" : _flagInfos[i].IsDummy ? " (DUMMY)" : "");

        setPos(0, true);
        LeftButton.OnInteract += ButtonPress(LeftButton, -1);
        RightButton.OnInteract += ButtonPress(RightButton, 1);
    }

    private KMSelectable.OnInteractHandler ButtonPress(KMSelectable btn, int offset)
    {
        return delegate
        {
            btn.AddInteractionPunch(.5f);
            Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, btn.transform);
            setPos(_curPos + offset);
            if (_submit != null)
                StopCoroutine(_submit);
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
            Module.HandlePass();
            _isSolved = true;
        }
        else if (!_flagInfos[_curPos].IsDummy)
        {
            Log("You stayed on position {0} for 10 seconds. Strike!", _curPos + 1);
            Module.HandleStrike();
        }
    }

    private void setPos(int pos, bool forced = false)
    {
        var newPos = Math.Max(0, Math.Min(7, pos));
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
            leftSema < 0 ? 0 : 180,
            rightSema > 0 ? 180 : 0));
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

    //#pragma warning disable 414
    //    private readonly string TwitchHelpMessage = @"!{0} ?";
    //#pragma warning restore 414

    //    public IEnumerator ProcessTwitchCommand(string command)
    //    {
    //        yield break;
    //    }

    //    IEnumerator TwitchHandleForcedSolve()
    //    {
    //        return null;
    //    }
}
