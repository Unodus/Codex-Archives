using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UnitConversions
{
	// Measurement constants
	private const float FEET_PER_METER = 3.28083989501312f;
	private const float METER_PER_FEET = 0.3048f;
	private const float FEET_PER_MILE = 5280f;
	private const float MILE_PER_FEET = 0.000189394f;
	private const float MPH_PER_KNOTS = 1.15077945f;
	private const float KNOTS_PER_MPH = 0.868976f;
	private const float METERS_PER_KM = 1000f;
	private const float KM_PER_METER = 0.001f;
	private const float METERS_PER_SECOND_PER_KNOT = 0.514444444f;
	private const float KNOT_PER_METERS_PER_SECOND = 1.94385f;
	private const float FEET_PER_SECOND_PER_KNOT = 1.68781f;
	private const float KNOT_PER_FEET_PER_SECOND = 0.592484f;
	private const float METERS_PER_SECOND_PER_MPH = 0.44704f;
	private const float MPH_PER_METERS_PER_SECOND = 0.44704f;
	private const float INCHES_PER_CM = 0.393701f;
	private const float CM_PER_INCHES = 2.54f;
	private const float SECONDS_TO_HOURS = 3600f;
	private const float HOURS_TO_SECONDS = 0.000277778f;

	// General constants
	public const float FLOATING_POINT_TOLERANCE = 0.00001f; // useful for comparisons between floating point numbers since accuracy cannot be guaranteed

	/// <summary>
	/// Centremetres go in, Inches come out 
	/// </summary>
	static public float CMToInches(float fCM)
	{
		return fCM * INCHES_PER_CM;
	}

	/// <summary>
	/// Inches go in, Centremetres come out 
	/// </summary>
	static public float InchesToCM(float fInches)
	{
		return fInches * CM_PER_INCHES;
	}

	/// <summary>
	/// Knots to MPH
	/// </summary>
	static public float KnotsToMPH(float fKnots)
	{
		return fKnots * MPH_PER_KNOTS;
	}

	static public float MPHToKnots(float fMPH)
	{
		return fMPH * KNOTS_PER_MPH;
	}

	static public float MetresPerSecondToMPH(float fMetersPerSecond)
	{
		return fMetersPerSecond * MPH_PER_METERS_PER_SECOND;
	}

	static public float MPHToMetresPerSecond(float fMPH)
	{
		return fMPH * METERS_PER_SECOND_PER_MPH;
	}

	static public float FeetPerSecondToMPH(float fFeetPerSecond)
	{
		return fFeetPerSecond * MILE_PER_FEET * SECONDS_TO_HOURS;
	}

	/// <summary>
	/// Feet go in, Metres come out
	/// </summary>
	static public float FeetToMetres(float fFeet)
	{
		return fFeet / FEET_PER_METER;
	}

	/// <summary>
	/// Feet go in, Metres come out
	/// </summary>
	static public Vector3 FeetToMetres(Vector3 feet)
	{
		return feet / FEET_PER_METER;
	}

	/// <summary>
	/// Metres go in, Feet come out
	/// </summary>
	static public float MetersToFeet(float fMeters)
	{
		return fMeters * FEET_PER_METER;
	}

	/// <summary>
	/// Metres go in, Feet come out
	/// </summary>
	static public Vector3 MetersToFeet(Vector3 meters)
	{
		return meters * FEET_PER_METER;
	}

	/// <summary>
	/// Miles go in, Feet come out
	/// </summary>
	static public float MilesToFeet(float fMiles)
	{
		return fMiles * FEET_PER_MILE;
	}

	/// <summary>
	/// Feet go in, Miles come out
	/// </summary>
	static public float FeetToMiles(float fFeet)
	{
		return fFeet / FEET_PER_MILE;
	}

	/// <summary>
	/// Meters go in, Kilometres come out
	/// </summary>
	static public float MetersToKM(float fMeters)
	{
		return fMeters * KM_PER_METER;
	}

	/// <summary>
	/// Kilometers go in, Metres come out
	/// </summary>
	static public float KMToMeters(float fKilometers)
	{
		return fKilometers * METERS_PER_KM;
	}

	/// <summary>
	/// Knots go in, Meters Per Second come out
	/// </summary>
	static public float KnotsToMPS(float fKnots)
	{
		// 1 knot = 0.514444444 metres per second
		return fKnots * METERS_PER_SECOND_PER_KNOT;
	}

	/// <summary>
	/// Meters Per Second go in, Knots come out
	/// </summary>
	static public float MPSToKnots(float fMetersPerSecond)
	{
		// 1 knot = 0.514444444 metres per second
		return fMetersPerSecond * KNOT_PER_METERS_PER_SECOND;
	}

	/// <summary>
	/// Knots go in, Metres Per Second come out
	/// </summary>
	static public float KnotsToFeetPerSecond(float fKnots)
	{
		return fKnots * FEET_PER_SECOND_PER_KNOT;
	}

	static public float FeetPerSecondToKnots(float fKnots)
	{
		return fKnots * KNOT_PER_FEET_PER_SECOND;
	}

}
